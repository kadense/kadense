using System.Text.Json;

namespace Kadense.Models.Malleable
{
    public class MalleableModuleFactory
    {
        public MalleableModuleFactory()
        {
        }

        protected string? GetMapping(string? typeName, IDictionary<string, MalleableTypeReference> mappings, string moduleNamespace, string moduleName)
        {
            if(typeName == null)
                return typeName;

            if(mappings.TryGetValue(typeName, out var baseTypeRef))
            {
                if(baseTypeRef.ModuleName == moduleName && baseTypeRef.ModuleNamespace == moduleNamespace)
                {
                    return baseTypeRef.ClassName;
                }
                else
                {
                    return baseTypeRef.GetQualifiedClassName();
                }
            }

            return typeName;
        }

        public IList<MalleableModule> SplitModule(MalleableModule module, int threshold = 50)
        {
            var modules = new List<MalleableModule>();
            if(module.Spec == null)
            {
                throw new ArgumentNullException(nameof(module.Spec));
            }
            if(module.Spec.Classes == null)
            {
                throw new ArgumentNullException(nameof(module.Spec.Classes));
            }
            if(module.Spec.Classes.Count == 0)
            {
                throw new ArgumentException("Module has no classes");
            }
            if(module.Spec.Classes.Count <= threshold)
            {
                modules.Add(module);
                return modules;
            }
            var added = module.Spec.SplitClasses();

            var mapping = new Dictionary<string, string>();

            foreach (var spec in added)
            {
                var newModule = new MalleableModule()
                {
                    Metadata = new k8s.Models.V1ObjectMeta()
                    {
                        NamespaceProperty = module.Metadata?.NamespaceProperty,
                        Name = $"{module.Metadata?.Name}-{modules.Count}"
                    },
                    Spec = spec
                };
                modules.Add(newModule);

                foreach (var @class in spec.Classes!)
                {
                    mapping.Add(@class.Key, newModule.GetQualifiedModuleName());
                }

                foreach (var @class in spec.Classes!)
                {
                    if (@class.Value.BaseClass != null)
                    {
                        @class.Value.BaseClass = GetModuleName(@class.Value.BaseClass, newModule, mapping);
                    }
                    if (@class.Value.DiscriminatorClass != null)
                    {
                        @class.Value.DiscriminatorClass = GetModuleName(@class.Value.DiscriminatorClass, newModule, mapping);
                    }
                    if (@class.Value.Properties != null)
                    {
                        foreach (var property in @class.Value.Properties)
                        {
                            if (property.Value.Type != null && property.Value.IsComplexType())
                            {
                                if(property.Value.IsCollectionType() || property.Value.IsDictionaryType())
                                {
                                    property.Value.SubType = GetModuleName(property.Value.SubType!, newModule, mapping);
                                }
                                else
                                {
                                    property.Value.Type = GetModuleName(property.Value.Type, newModule, mapping);
                                }
                            }
                        }
                    }
                }
            }

            

            return modules;
        }

        protected string GetModuleName(string className, MalleableModule module, IDictionary<string, string> mapping)
        {
            if(className.Contains(":"))
                return className;

            var moduleName = mapping[className];
            if (moduleName == module.GetQualifiedModuleName())
            {
                return className;
            }
            else
            {
                return $"{moduleName}:{className}";
            }
        }

        public MalleableModule FromFhirSchema(string jsonSchema, string @namespace, string name)
        {
            var module = new MalleableModule();
            module.Metadata.NamespaceProperty = @namespace;
            module.Metadata.Name = name;
            module.Spec = new MalleableModuleSpec();
            module.Spec.Classes = new Dictionary<string, MalleableClass>();
            var resourceListOptions = new List<string>();


            JsonDocument jsonDocument = JsonDocument.Parse(jsonSchema);
            JsonElement root = jsonDocument.RootElement;
            if (root.TryGetProperty("definitions", out JsonElement definitions))
            {
                definitions.EnumerateObject().ToList().ForEach(definition =>
                {
                    var malleableClass = new MalleableClass();
                    malleableClass.Properties = new Dictionary<string, MalleableProperty>();
                    if (definition.Name == "ResourceList")
                    {
                        if (definition.Value.TryGetProperty("oneOf", out JsonElement oneOf))
                        {
                            oneOf.EnumerateArray().ToList().ForEach(oneOfItem =>
                            {
                                if (oneOfItem.TryGetProperty("$ref", out JsonElement refElement))
                                {
                                    string refName = this.GetTypeName(refElement);
                                    resourceListOptions.Add(refName);
                                }
                            });
                        }
                    }
                    else
                    {
                        if (definition.Value.TryGetProperty("allOf", out JsonElement allOf))
                        {
                            allOf.EnumerateArray().ToList().ForEach(allOfItem =>
                            {
                                if (allOfItem.TryGetProperty("$ref", out JsonElement refElement))
                                {
                                    string refName = this.GetTypeName(refElement);
                                }
                                if (allOfItem.TryGetProperty("description", out JsonElement description))
                                {
                                    malleableClass.Description = description.GetString();
                                }

                                if (allOfItem.TryGetProperty("properties", out JsonElement properties))
                                {
                                    properties.EnumerateObject().ToList().ForEach(property =>
                                    {
                                        var malleableProperty = new MalleableProperty();

                                        if (property.Value.TryGetProperty("$ref", out JsonElement typeRef))
                                        {
                                            malleableProperty.Type = this.GetTypeName(typeRef);
                                        }
                                        else if (property.Value.TryGetProperty("type", out JsonElement type))
                                        {
                                            malleableProperty.Type = this.GetTypeName(type);
                                        }

                                        if (property.Value.TryGetProperty("items", out JsonElement items))
                                        {
                                            if (items.TryGetProperty("$ref", out JsonElement itemsRef))
                                            {
                                                malleableProperty.SubType = this.GetTypeName(itemsRef);
                                            }
                                            else if (items.TryGetProperty("type", out JsonElement itemsType))
                                            {
                                                malleableProperty.SubType = this.GetTypeName(itemsType);
                                            }
                                        }

                                        if (property.Value.TryGetProperty("enum", out JsonElement enumRef))
                                        {
                                            malleableProperty.PropertyEnum = new List<string>();
                                            enumRef.EnumerateArray().ToList().ForEach(enumItem =>
                                            {
                                                malleableProperty.PropertyEnum.Add(enumItem.GetString()!);
                                            });
                                        }

                                        if (property.Value.TryGetProperty("description", out JsonElement propertyDescription))
                                        {
                                            malleableProperty.Description = propertyDescription.GetString();
                                        }

                                        malleableClass.Properties.Add(property.Name, malleableProperty);
                                    });
                                }
                            });
                        }
                    }
                    module.Spec.Classes.Add(definition.Name, malleableClass);
                });
            }

            if (resourceListOptions.Count > 0)
            {
                var resourceListMapping = new Dictionary<string, string>();
                foreach (var resourceListOption in resourceListOptions)
                {
                    if (resourceListOption != "Resource")
                    {
                        if (module.Spec.Classes.ContainsKey(resourceListOption))
                        {
                            var malleableClass = module.Spec.Classes[resourceListOption];
                            malleableClass.DiscriminatorClass = "Resource";
                            if (malleableClass.Properties!.ContainsKey("resourceType"))
                            {
                                malleableClass.TypeDiscriminator = malleableClass.Properties["resourceType"].PropertyEnum!.First();
                                malleableClass.Properties!.Remove("resourceType");
                            }
                        }
                    }
                }
                module.Spec.Classes["Resource"].DiscriminatorProperty = "resourceType";
            }

            return module;
        }

        protected string GetTypeName(JsonElement type)
        {
            var typeRef = type.GetString()!;
            string typeName = string.Empty;
            if(typeRef.StartsWith("#/definitions/"))
            {
                typeName = typeRef.Substring("#/definitions/".Length);
            }
            else
            {
                typeName = typeRef;
            }
            switch(typeName)
            {
                case "ResourceList":
                    typeName = "Resource";
                    break;
                case "boolean":
                    typeName = "bool";
                    break;
                case "integer":
                    typeName = "int";
                    break;
                case "number":
                    typeName = "double";
                    break;
            }
            return typeName;
        }
    }
}