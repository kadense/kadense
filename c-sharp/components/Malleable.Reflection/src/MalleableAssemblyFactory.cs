using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Kadense.Models.Kubernetes;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Reflection
{
    public class MalleableAssemblyFactory
    {
        public MalleableAssemblyFactory()
        {
            ExpressionParameters = new Dictionary<string, Type>();
            Assemblies = new MalleableAssemblyCollection();
        }

        protected MalleableAssemblyCollection Assemblies { get; } 

        protected IDictionary<string, Type> ExpressionParameters { get; }

        public MalleableAssemblyFactory WithExpressionParameter<T>(string name)
        {
            ExpressionParameters.Add(name, typeof(T));
            return this;
        }

        public MalleableAssemblyCollection GetAssemblies()
        {
            return Assemblies;
        }

        public MalleableAssemblyFactory WithAssemblies(IList<MalleableModule> modules)
        {
            var list = modules.ToList();
            list.Sort();
            foreach(var module in modules)
            {
                WithAssembly(module);
            }
            return this;
        }

        public MalleableAssemblyFactory WithAssembly(MalleableModule module)
        {
            WithNewAssembly(module);
            return this;
        } 
        public MalleableAssembly WithNewAssembly(MalleableModule module)
        {
            module.Spec!.GetReferencedModules().ToList().ForEach(x => {
                if(!Assemblies.ContainsKey(x))
                {
                    throw new ArgumentException($"Referenced module {x} not found in assemblies");
                } 
            });
            var assembly = CreateAssembly(module);
            Assemblies.Add(assembly.Name, assembly);
            return assembly;
        } 

        public MalleableAssemblyFactory WithAssembly(MalleableConverterModule module)
        {
            WithNewAssembly(module);
            return this;
        } 

        public MalleableAssembly WithNewAssembly(MalleableConverterModule module)
        {
            var assembly = CreateAssembly(module, Assemblies);
            Assemblies.Add(assembly.Name, assembly);
            return assembly;
        } 

        protected MalleableAssembly CreateAssembly(MalleableConverterModule module, IDictionary<string, MalleableAssembly> involvedAssemblies)
        {
            return CreateAssembly(module.Metadata.NamespaceProperty, module.Metadata.Name, module.Spec!, involvedAssemblies);
        }
        protected MalleableAssembly CreateAssembly(string @namespace, string name, MalleableConverterModuleSpec moduleSpec, IDictionary<string, MalleableAssembly> involvedAssemblies)
        {
            var assemblyName = new AssemblyName(name);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            
            var typeBuilder = DefineConverters(moduleBuilder, @namespace, name, moduleSpec, involvedAssemblies);
            var assembly = CompileTypes($"{@namespace}:{name}", typeBuilder);
            return assembly;            
        }

        protected IDictionary<string, TypeBuilder> DefineConverters(ModuleBuilder moduleBuilder, string moduleNamespace, string moduleName, MalleableConverterModuleSpec moduleSpec, IDictionary<string, MalleableAssembly> InvolvedAssemblies)
        {
            var builders = new Dictionary<string, TypeBuilder>();

            foreach (var converter in moduleSpec.Converters)
            {
                var converterType = DefineConverter(moduleBuilder, moduleNamespace, moduleName, converter.Key, converter.Value, InvolvedAssemblies);
                builders.Add(converter.Key, converterType);
            }
            return builders;
        }

        protected TypeBuilder DefineConverter(ModuleBuilder moduleBuilder, string moduleNamespace, string moduleName, string name, MalleableTypeConverter converterSpec, IDictionary<string, MalleableAssembly> InvolvedAssemblies)
        {
            var fromQualifiedName = converterSpec.From!.GetQualifiedModuleName();
            var toQualifiedName = converterSpec.To!.GetQualifiedModuleName();
            var fromAssembly = InvolvedAssemblies[fromQualifiedName];
            var toAssembly = InvolvedAssemblies[toQualifiedName];
            var fromType = fromAssembly.Types[converterSpec.From.ClassName!];
            var toType = toAssembly.Types[converterSpec.To.ClassName!];
            var fromTypeAttribute = MalleableClassAttribute.FromType(fromType);
            var convertFromModuleNamespace = fromTypeAttribute.ModuleNamespace;
            var convertFromModuleName = fromTypeAttribute.ModuleName;
            var convertFromClassName = fromTypeAttribute.ClassName;
            
            var toTypeAttribute = MalleableClassAttribute.FromType(toType);
            var convertToModuleNamespace = toTypeAttribute.ModuleNamespace;
            var convertToModuleName = toTypeAttribute.ModuleName;
            var convertToClassName = toTypeAttribute.ClassName;

            
            var parentType = typeof(MalleableConverterBase<,>).MakeGenericType(new Type[] { fromType, toType });
            var typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public, parentType);
            
            var customAttributeBuilder = new CustomAttributeBuilder(
                typeof(MalleableConverterAttribute).GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) })!,
                new object[] { moduleNamespace, moduleName, name, convertFromModuleNamespace, convertFromModuleName, convertFromClassName, convertToModuleNamespace, convertToModuleName, convertToClassName }
            );
            typeBuilder.SetCustomAttribute(customAttributeBuilder);

            var baseConstructor = parentType.GetConstructor(new Type[] { typeof(IDictionary<string, object>) })!;
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName, CallingConventions.Standard, new Type[] { typeof(IDictionary<string, object>) });
            var constructorIL = constructor.GetILGenerator();
            
            // Call the base constructor
            constructorIL.Emit(OpCodes.Ldarg_0);
            constructorIL.Emit(OpCodes.Ldarg_1);
            constructorIL.Emit(OpCodes.Call, baseConstructor);

            // Get the dictionary of expressions
            var expressions = converterSpec.Expressions;
            
            // Define a readonly field for the dictionary
            var expressionDelegatesField = typeBuilder.DefineField($"_{name}_delegates", typeof(IDictionary<string, Delegate>), FieldAttributes.Private | FieldAttributes.InitOnly);

            // non-static field initialization
            constructorIL.Emit(OpCodes.Ldarg_0); // Load 'this' onto the stack
            constructorIL.Emit(OpCodes.Newobj, typeof(Dictionary<string, Delegate>).GetConstructor(Type.EmptyTypes)!);
            constructorIL.Emit(OpCodes.Stfld, expressionDelegatesField); // Use Stfld for non-static fields
            var compileMethod = parentType.GetMethod("CompileExpression", BindingFlags.NonPublic | BindingFlags.Instance)!;

            foreach (var kvp in expressions)
            {
                constructorIL.Emit(OpCodes.Ldarg_0); // Load 'this' onto the stack
                constructorIL.Emit(OpCodes.Ldstr, kvp.Key);
                constructorIL.Emit(OpCodes.Ldstr, kvp.Value);
                constructorIL.Emit(OpCodes.Ldarg_0); // Load 'this' onto the stack
                constructorIL.Emit(OpCodes.Ldfld, expressionDelegatesField); // Use Ldfld for non-static fields
                constructorIL.Emit(OpCodes.Call, compileMethod); // Call the static Convert method
            }

            // Define the method to convert from the source type to the destination type
            var method = typeBuilder.DefineMethod($"Convert", MethodAttributes.Public | MethodAttributes.Virtual, toType, new Type[] { fromType });
            typeBuilder.DefineMethodOverride(method, parentType.GetMethod("Convert", BindingFlags.Public | BindingFlags.Instance)!);
            var methodIL = method.GetILGenerator();
            var convertMethod = parentType.GetMethod("Convert", BindingFlags.NonPublic | BindingFlags.Instance )!;

            methodIL.Emit(OpCodes.Ldarg_0); // Load 'this' onto the stack
            methodIL.Emit(OpCodes.Ldarg_1); // Load the fromObject argument
            methodIL.Emit(OpCodes.Ldarg_0); // Load 'this' onto the stack
            methodIL.Emit(OpCodes.Ldfld, expressionDelegatesField); // Use Ldfld for non-static fields
            methodIL.Emit(OpCodes.Call, convertMethod); // Call the static Convert method
            methodIL.Emit(OpCodes.Ret);

            constructorIL.Emit(OpCodes.Ret);
            return typeBuilder;
        }
        protected MalleableAssembly CreateAssembly(MalleableModule module)
        {
            return CreateAssembly(module.Metadata.Name, module.Metadata.NamespaceProperty, module.Spec!);
        }

        protected MalleableAssembly CreateAssembly(string name, string @namespace, MalleableModuleSpec moduleSpec)
        {
            var assemblyName = new AssemblyName(name!);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            var buildOrder = moduleSpec.GetClassesForBuild();
            var reflectedTypes = DefineTypeBuilders(moduleBuilder, buildOrder);
            PrepareTypes(moduleBuilder, buildOrder, reflectedTypes, @namespace, name);
            return CompileTypes($"{@namespace}:{name}", buildOrder, reflectedTypes);
        }
            
        /// <summary>
        /// Create the type builders for the classes in the module spec, but do not build them yet as they may refer to each other. 
        /// </summary>
        protected virtual IDictionary<string, TypeBuilder> DefineTypeBuilders(ModuleBuilder moduleBuilder, IList<KeyValuePair<string,MalleableClass>> buildOrder)
        {   
            var reflectedTypes = new Dictionary<string, TypeBuilder>();         
            foreach (var typeDefinition in buildOrder)
            {
                DefineTypeBuilder(moduleBuilder, typeDefinition.Key, typeDefinition.Value, reflectedTypes);
            }
            return reflectedTypes;
        }

        protected virtual (bool, Type) GetParentType(MalleableClass typeDefinition,  IDictionary<string, TypeBuilder> reflectedTypes)
        {
            if(!string.IsNullOrEmpty(typeDefinition.BaseClass))
            {
                var (isRefType, typeRef) = typeDefinition.TryGetBaseClassReference();
                if(isRefType)
                {
                    return (true, this.Assemblies[typeRef!.GetQualifiedModuleName()].Types[typeRef.ClassName!]);
                }
                else
                {
                    return (false, reflectedTypes[typeDefinition.BaseClass!]);
                }
            }
            return (true, typeof(MalleableBase));
        }

        /// <summary>
        /// Create the type builder for a class in the module spec, but do not build them yet as they may reference each other.
        /// </summary>
        protected virtual void DefineTypeBuilder(ModuleBuilder moduleBuilder, string name, MalleableClass typeDefinition,  IDictionary<string, TypeBuilder> reflectedTypes )
        {   
            var (parentTypeIsRef, parentType) = GetParentType(typeDefinition, reflectedTypes);
            var typeBuilder = moduleBuilder.DefineType(name!, System.Reflection.TypeAttributes.Public, parentType);            
            
            reflectedTypes.Add(name, typeBuilder);
        }

        /// <summary>
        /// Prepare the types for the module spec. This will create the constructors, and define the properties for the types.
        /// </summary>
        protected virtual void PrepareTypes(ModuleBuilder moduleBuilder, IList<KeyValuePair<string,MalleableClass>> buildOrder, IDictionary<string, TypeBuilder> reflectedTypes, string @namespace, string name)
        {
            var reflectedConstructors = new Dictionary<string, ConstructorBuilder>();
            foreach (var typeDefinition in buildOrder)
            {
                PrepareType(moduleBuilder, typeDefinition.Key, typeDefinition.Value, reflectedTypes, reflectedConstructors, @namespace, name);                
            }
        }

        /// <summary>
        /// Prepare the type for the module spec. This will create the constructors, and define the properties for the types.
        /// </summary>
        protected virtual void PrepareType(ModuleBuilder moduleBuilder, string name, MalleableClass typeDefinition, IDictionary<string, TypeBuilder> reflectedTypes, IDictionary<string, ConstructorBuilder> reflectedConstructors, string moduleNamespace, string moduleName)
        {
            var typeBuilder = reflectedTypes[name];
            var (parentTypeIsRef, parentType) = GetParentType(typeDefinition, reflectedTypes);
            var parentConstructor = parentTypeIsRef ? parentType.GetConstructor(new Type[0]) : reflectedConstructors[typeDefinition.BaseClass!];
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            reflectedConstructors.Add(name, constructorBuilder);
            var constructorIL = constructorBuilder.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);
            constructorIL.Emit(OpCodes.Call, parentConstructor!);
            PrepareProperties(typeBuilder, typeDefinition, constructorIL, reflectedTypes);
            constructorIL.Emit(OpCodes.Ret);

            var customAttributeBuilder = new CustomAttributeBuilder(
                typeof(MalleableClassAttribute).GetConstructor(new[] { typeof(string), typeof(string), typeof(string) })!,
                new object[] { moduleNamespace, moduleName, name }
            );
            typeBuilder.SetCustomAttribute(customAttributeBuilder);

            if(!string.IsNullOrEmpty(typeDefinition.IdentifierExpression))
            {
                ImplementIdentifierExpression(typeBuilder,typeDefinition.IdentifierExpression);
            }
        }

        protected void ImplementIdentifierExpression(TypeBuilder typeBuilder, string expression)
        {
            var compileStringExpressionMethod = typeof(MalleableBase).GetMethod("CompileStringExpression", BindingFlags.Public | BindingFlags.Static, new [] { typeof(string) })!
                .MakeGenericMethod(new Type[] { typeBuilder });

        
            // Add the identifier expression static field
            var identifierField = typeBuilder.DefineField($"_identifierExpression", typeof(Func<,>).MakeGenericType(new Type[] { typeBuilder, typeof(string)}), FieldAttributes.Public | FieldAttributes.Static);

            // Define a static constructor to initialize the static field
            var staticConstructor = typeBuilder.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
            var staticConstructorIL = staticConstructor.GetILGenerator();

            // Initialize the static field in the static constructor
            staticConstructorIL.Emit(OpCodes.Ldstr, expression); // Load the expression string onto the stack
            staticConstructorIL.Emit(OpCodes.Call, compileStringExpressionMethod); // Call CompileStringExpressionMethod to get the delegate
            staticConstructorIL.Emit(OpCodes.Stsfld, identifierField); // Store the delegate in the static field
            staticConstructorIL.Emit(OpCodes.Ret);

            // Implement the interface IMalleableIdentifiable
            var identifiableInterface = typeof(IMalleableIdentifiable);
            typeBuilder.AddInterfaceImplementation(identifiableInterface);

            // Implement the GetIdentifier method from IMalleableIdentifiable
            var getIdentifierMethod = typeBuilder.DefineMethod("GetIdentifier", MethodAttributes.Public | MethodAttributes.Virtual, typeof(string), Type.EmptyTypes);
            var il = getIdentifierMethod.GetILGenerator();
            var invokeMethod = typeof(MalleableBase).GetMethod("GetExpressionResult", BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(typeBuilder);
            // Load the delegate from the static field and invoke it
            il.Emit(OpCodes.Ldarg_0); // Load 'this' onto the stack
            il.Emit(OpCodes.Ldsfld, identifierField); // Load identifierField
            il.Emit(OpCodes.Call, invokeMethod); // Call Invoke on the delegate
            il.Emit(OpCodes.Ret); // Return the string value

            typeBuilder.DefineMethodOverride(getIdentifierMethod, identifiableInterface.GetMethod("GetIdentifier")!);
        }

        protected virtual void PrepareProperties(TypeBuilder typeBuilder, MalleableClass typeDefinition, ILGenerator constructorIL, IDictionary<string, TypeBuilder> reflectedTypes)
        {
            foreach(var property in typeDefinition.Properties!)
            {
                PrepareProperty(typeBuilder, property.Key, property.Value, constructorIL, reflectedTypes);                
            }
        }

        protected virtual PropertyBuilder PrepareProperty(TypeBuilder typeBuilder, string name, MalleableProperty propertyDefinition, ILGenerator constructorIL, IDictionary<string, TypeBuilder> reflectedTypes)
        {
            var returnType = GetPropertyType(propertyDefinition, reflectedTypes);
            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, returnType, null);
            var fieldBuilder = typeBuilder.DefineField($"_{name}", returnType, FieldAttributes.Private);
            AddBasicGetMethod(typeBuilder, propertyBuilder, fieldBuilder);
            AddBasicSetMethod(typeBuilder, propertyBuilder, fieldBuilder);
            return propertyBuilder;
        }

        protected static void AddBasicGetMethod(TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod($"get_{propertyBuilder.Name}", MethodAttributes.Public, propertyBuilder.PropertyType, Type.EmptyTypes);
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuilder);
            il.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(methodBuilder);
        }

        protected static void AddBasicSetMethod(TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod($"set_{propertyBuilder.Name}", MethodAttributes.Public, null, new Type[] { propertyBuilder.PropertyType });
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldBuilder);
            il.Emit(OpCodes.Ret);

            propertyBuilder.SetSetMethod(methodBuilder);
        }

        protected virtual Type GetPrimitiveType(string type)
        {
            switch(type.ToLower())
            {
                case "string":
                    return typeof(string);
                case "int":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "double":
                    return typeof(double);
                case "bool":
                    return typeof(bool);
                case "short":
                    return typeof(short);
                case "char":
                    return typeof(char);
                case "byte":
                    return typeof(byte);
                case "ulong":
                    return typeof(ulong);
                case "sbyte":
                    return typeof(sbyte);
                case "float":
                    return typeof(float);
                case "decimal":
                    return typeof(decimal);
                case "datetime":
                    return typeof(DateTime);
                case "datetimeoffset":
                    return typeof(DateTimeOffset);
                case "timespan":
                    return typeof(TimeSpan);
                default:
                    throw new ArgumentException($"Unknown primitive type: {type}");
            }
        }
        protected virtual Type GetPropertyType(MalleableProperty propertyDefinition, IDictionary<string, TypeBuilder> reflectedTypes)
        {
            if (propertyDefinition.IsPrimitiveType())
            {
                return GetPrimitiveType(propertyDefinition.Type);
            }
            else if (propertyDefinition.IsDictionaryType())
            {
                if (propertyDefinition.IsPrimitiveSubType())
                {
                    return typeof(Dictionary<,>).MakeGenericType(typeof(string), Type.GetType(propertyDefinition.SubType ?? "string")!);
                }
                else
                {
                    (var isRefType, var typeRef) = propertyDefinition.TryGetSubTypeReference();
                    if(!isRefType)
                        return typeof(Dictionary<,>).MakeGenericType(typeof(string), reflectedTypes[propertyDefinition.SubType!]);

                    var subType = this.Assemblies[typeRef!.GetQualifiedModuleName()].Types[typeRef.ClassName!];
                    return typeof(Dictionary<,>).MakeGenericType(typeof(string), subType);
                }
            }
            else if (propertyDefinition.IsCollectionType())
            {
                
                if (propertyDefinition.IsPrimitiveSubType())
                {
                    return typeof(List<>).MakeGenericType(GetPrimitiveType(propertyDefinition.SubType ?? "string")!);
                }
                else
                {
                    (var isRefType, var typeRef) = propertyDefinition.TryGetSubTypeReference();
                    
                    if(!isRefType)
                        return typeof(List<>).MakeGenericType(reflectedTypes[propertyDefinition.SubType!]);
                    var subType = this.Assemblies[typeRef!.GetQualifiedModuleName()].Types[typeRef.ClassName!];
                    return typeof(List<>).MakeGenericType(subType);
                }
            }
            else
            {
                (var isRefType, var typeRef) = propertyDefinition.TryGetTypeReference();
                
                if(!isRefType)
                    return reflectedTypes[propertyDefinition.Type!];

                var type = this.Assemblies[typeRef!.GetQualifiedModuleName()].Types[typeRef.ClassName!];
                return type;
            }
        }

        protected virtual MalleableAssembly CompileTypes(string name, IList<KeyValuePair<string,MalleableClass>> buildOrder, IDictionary<string, TypeBuilder> reflectedTypes)
        {
            var assembly = new MalleableAssembly(name);
            
            foreach (var typeDefinition in buildOrder)
            {
                var typeBuilder = reflectedTypes[typeDefinition.Key];
                var type = typeBuilder.CreateType();
                assembly.AddType(typeDefinition.Key, type);     
                
                if(!string.IsNullOrEmpty(typeDefinition.Value.DiscriminatorProperty))
                {
                    var options = new MalleableJsonPolymorphicOptions()
                    {
                        TypeDiscriminatorPropertyName = typeDefinition.Value.DiscriminatorProperty,
                    };
                    assembly.JsonPolymorphicOptions.Add(type, options);
                }

                if(!string.IsNullOrEmpty(typeDefinition.Value.BaseClass) && !string.IsNullOrEmpty(typeDefinition.Value.TypeDiscriminator))
                {
                    var (isRefType, typeRef) = typeDefinition.Value.TryGetBaseClassReference();
                    var baseType = isRefType ? Assemblies[typeRef!.GetQualifiedModuleName()].Types[typeRef.ClassName!]  : assembly.Types[typeDefinition.Value.BaseClass];
                    
                    
                    if(!string.IsNullOrEmpty(typeDefinition.Value.DiscriminatorClass))
                    {
                        var (isDiscriminatorRefType, discriminatorTypeRef) = typeDefinition.Value.TryGetDiscriminatorClassReference();
                        baseType = isDiscriminatorRefType ? Assemblies[discriminatorTypeRef!.GetQualifiedModuleName()].Types[discriminatorTypeRef.ClassName!] : assembly.Types[typeDefinition.Value.DiscriminatorClass];
                    }

                    MalleableJsonPolymorphicOptions? options = null;
                    if(!assembly.JsonPolymorphicOptions.TryGetValue(baseType, out options))
                    {
                        options = new MalleableJsonPolymorphicOptions();
                        assembly.JsonPolymorphicOptions.Add(baseType, options);
                    }
                    options.DerivedTypes.Add(new JsonDerivedType(type, typeDefinition.Value.TypeDiscriminator));
                }
            }
            return assembly;
        }

        
        protected virtual MalleableAssembly CompileTypes(string name, IDictionary<string, TypeBuilder> reflectedTypes)
        {
            var assembly = new MalleableAssembly(name);
            
            foreach (var typeDefinition in reflectedTypes)
            {
                var typeBuilder = reflectedTypes[typeDefinition.Key];
                var type = typeBuilder.CreateType();
                assembly.AddType(typeDefinition.Key, type);     
            }

            foreach(var expressionParameter in ExpressionParameters)
            {
                assembly.ExpressionParameters.Add(expressionParameter.Key, expressionParameter.Value);
            }

            return assembly;
        }
    }
}