using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Kadense.Models.Kubernetes;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Reflection
{
    public class MalleableAssemblyFactory
    {
        public MalleableAssembly CreateAssembly(MalleableModule module)
        {
            return CreateAssembly(module.Metadata.Name, module.Spec!);
        }

        public MalleableAssembly CreateAssembly(string name, MalleableModuleSpec moduleSpec)
        {
            var assemblyName = new AssemblyName(name!);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            var buildOrder = CalculateBuildOrder(moduleSpec);
            var reflectedTypes = DefineTypeBuilders(moduleBuilder, buildOrder);
            PrepareTypes(moduleBuilder, buildOrder, reflectedTypes);
            return CompileTypes(name, buildOrder, reflectedTypes);
        }

        protected virtual IList<KeyValuePair<string,MalleableClass>> CalculateBuildOrder(MalleableModuleSpec moduleSpec)
        {
            var buildOrder = new List<KeyValuePair<string,MalleableClass>>();
            if(moduleSpec.Classes == null)
                throw new ArgumentNullException(nameof(moduleSpec.Classes));

            var added = moduleSpec.Classes.Where(x => x.Value.BaseClass == null);
            buildOrder.AddRange(added);
            do 
            {
                added = moduleSpec.Classes.Where(
                    x => x.Value.BaseClass != null
                    && buildOrder.Select(y => y.Key).Contains(x.Value.BaseClass) 
                    && !buildOrder.Select(y => y.Key).Contains(x.Key));
                buildOrder.AddRange(added);
            } 
            while (added.Count() > 0);


            if(buildOrder.Count != moduleSpec.Classes.Count)
            {
                var missing = moduleSpec.Classes.Where(x => !buildOrder.Select(y => y.Key).Contains(x.Key));
                throw new ArgumentException($"Missing classes in build order: {string.Join(", ", missing.Select(x => x.Key))}");
            }

            return buildOrder;
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

        /// <summary>
        /// Create the type builder for a class in the module spec, but do not build them yet as they may reference each other.
        /// </summary>
        protected virtual void DefineTypeBuilder(ModuleBuilder moduleBuilder, string name, MalleableClass typeDefinition,  IDictionary<string, TypeBuilder> reflectedTypes )
        {   
            var parentType = !string.IsNullOrEmpty(typeDefinition.BaseClass) ? reflectedTypes[typeDefinition.BaseClass!] : typeof(MalleableBase);
            var typeBuilder = moduleBuilder.DefineType(name!, System.Reflection.TypeAttributes.Public, parentType);            
            reflectedTypes.Add(name, typeBuilder);
        }

        /// <summary>
        /// Prepare the types for the module spec. This will create the constructors, and define the properties for the types.
        /// </summary>
        protected virtual void PrepareTypes(ModuleBuilder moduleBuilder, IList<KeyValuePair<string,MalleableClass>> buildOrder, IDictionary<string, TypeBuilder> reflectedTypes)
        {
            var reflectedConstructors = new Dictionary<string, ConstructorBuilder>();
            foreach (var typeDefinition in buildOrder)
            {
                PrepareType(moduleBuilder, typeDefinition.Key, typeDefinition.Value, reflectedTypes, reflectedConstructors);                
            }
        }

        /// <summary>
        /// Prepare the type for the module spec. This will create the constructors, and define the properties for the types.
        /// </summary>
        protected virtual void PrepareType(ModuleBuilder moduleBuilder, string name, MalleableClass typeDefinition, IDictionary<string, TypeBuilder> reflectedTypes, IDictionary<string, ConstructorBuilder> reflectedConstructors)
        {
            var typeBuilder = reflectedTypes[name];
            var parentConstructor = !string.IsNullOrEmpty(typeDefinition.BaseClass) ? reflectedConstructors[typeDefinition.BaseClass] : typeof(object).GetConstructor(new Type[0]);
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            reflectedConstructors.Add(name, constructorBuilder);
            var constructorIL = constructorBuilder.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);
            constructorIL.Emit(OpCodes.Call, parentConstructor!);
            PrepareProperties(typeBuilder, typeDefinition, constructorIL, reflectedTypes);
            constructorIL.Emit(OpCodes.Ret);
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

        private static void AddBasicGetMethod(TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod($"get_{propertyBuilder.Name}", MethodAttributes.Public, propertyBuilder.PropertyType, Type.EmptyTypes);
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuilder);
            il.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(methodBuilder);
        }

        private static void AddBasicSetMethod(TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder)
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
                return GetPrimitiveType(propertyDefinition.PropertyType);
            }
            else if (propertyDefinition.IsDictionaryType())
            {
                if (propertyDefinition.IsPrimitiveSubType())
                {
                    return typeof(Dictionary<,>).MakeGenericType(typeof(string), Type.GetType(propertyDefinition.SubType ?? "string")!);
                }
                else
                {
                    return typeof(Dictionary<,>).MakeGenericType(typeof(string), reflectedTypes[propertyDefinition.SubType!]);
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
                    return typeof(List<>).MakeGenericType(reflectedTypes[propertyDefinition.SubType!]);
                }
            }
            else
            {
                return reflectedTypes[propertyDefinition.PropertyType];
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
            }
            return assembly;
        }
    }
}