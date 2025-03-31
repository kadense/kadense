using k8s.Models;
using System.Diagnostics;
using System.Reflection;

namespace Kadense.Client.Kubernetes
{
    public class CustomResourceDefinitionFactory
    {
        public IDictionary<string, V1JSONSchemaProps> ProcessProperties(Type type){
            var properties = new Dictionary<string, V1JSONSchemaProps>();

            foreach (var property in type.GetProperties())
            {
                var ignoreFlags = property.GetCustomAttributes(typeof(IgnoreOnCrdGenerationAttribute), false);
                if(ignoreFlags.Length == 0)
                {
                    string propertyName = property.Name;
                    if (propertyName.Length > 1)
                    {
                        propertyName = $"{propertyName.Substring(0, 1).ToLower()}{propertyName.Substring(1)}";
                    }
                    var jsonPropNames = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false);
                    if (jsonPropNames.Length > 0)
                    {
                        propertyName = ((JsonPropertyNameAttribute)jsonPropNames[0]).Name;
                    }
                    properties.Add(propertyName, ProcessProperty(property));
                }
            }

            return properties;
        }

        public V1JSONSchemaProps ProcessProperty(PropertyInfo property){
            return ProcessProperty(property.PropertyType);
        }

        public V1JSONSchemaProps ProcessProperty(Type propertyType){
            V1JSONSchemaProps result = new V1JSONSchemaProps();

            if (propertyType.Equals(typeof(string)))
            {
                result.Type = "string";
            }
            else if (propertyType.Equals(typeof(int)))
            {
                result.Type = "integer";
            }
            else if (propertyType.IsEnum)
            {
                result.Type = "string";
                result.EnumProperty = new List<object>();
                foreach (string? enumName in propertyType.GetEnumNames())
                {
                    result.EnumProperty.Add(enumName);
                }
            }
            else if (propertyType.IsArray)
            {
                // Array types are not yet implemented.
                throw new NotImplementedException();
            }
            else if (propertyType.IsGenericType)
            {
                Type genericTypeDefinition = propertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>)) 
                {
                    result.Type = "object";
                    var additionalProperties = new V1JSONSchemaProps();


                    if (propertyType.GetGenericArguments()[0] != typeof(string))
                        throw new NotImplementedException();

                    if (propertyType.GetGenericArguments()[1] == typeof(string))
                    {
                        additionalProperties.Type = "string";
                    }
                    else
                    {
                        additionalProperties.Type = "object";
                        additionalProperties.Properties = ProcessProperties(propertyType);
                    }
                    result.AdditionalProperties = additionalProperties;                    
                }
                else if(genericTypeDefinition == typeof(List<>))
                {
                    result.Type = "array";
                    var items = new V1JSONSchemaProps();
                    
                    
                    if (propertyType.GetGenericArguments()[0] == typeof(string))
                    {
                        items.Type = "string";
                    }
                    else
                    {
                        items.Type = "object";
                        items.Properties = ProcessProperties(propertyType);
                    }
                    result.Items = items;
                }
                else if(genericTypeDefinition == typeof(Nullable<>))
                {
                    return ProcessProperty(propertyType.GetGenericArguments()[0]);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                // For complex types, recursively process their properties.
                result.Type = "object";
                result.Properties = ProcessProperties(propertyType);
            }
            return result;
        }

        public V1CustomResourceDefinition Create<T>(){
            Type type = typeof(T);
            var attribute = (KubernetesCustomResourceAttribute)type.GetCustomAttributes(typeof(KubernetesCustomResourceAttribute), false).First();

            var names = new V1CustomResourceDefinitionNames(){
                Kind = attribute.Kind,
                Plural = attribute.PluralName.ToLower(),
                Singular = attribute.Kind.ToLower(),
            };
            
            var spec = new V1CustomResourceDefinitionSpec(){
                Scope = "Namespaced",
                Group = attribute.Group,
                Names = names        
            };

            var categories = type.GetCustomAttributes(typeof(KubernetesCategoryNameAttribute), false);
            if (categories.Length > 0)
            {
                var categoryNames = categories.Select(c => ((KubernetesCategoryNameAttribute)c).Name).ToList();
                spec.Names.Categories = categoryNames;
            }

            var shortNames = type.GetCustomAttributes(typeof(KubernetesShortNameAttribute), false);
            if (shortNames.Length > 0)
            {
                var shortNameList = shortNames.Select(c => ((KubernetesShortNameAttribute)c).Name).ToList();
                spec.Names.ShortNames = shortNameList;
            }

            spec.Versions = new List<V1CustomResourceDefinitionVersion>(){
                new V1CustomResourceDefinitionVersion(){
                    Name = attribute.Version,
                    Served = true,
                    Storage = true,
                    Schema = new V1CustomResourceValidation(){
                        OpenAPIV3Schema = new V1JSONSchemaProps(){
                            Type = "object",
                            Properties = ProcessProperties(type)
                        }
                    }
                }
            };



            return new V1CustomResourceDefinition(){
                Spec = spec,
                Metadata = new V1ObjectMeta(){
                    Name = $"{attribute.PluralName.ToLower()}.{attribute.Group}"
                }
            };
        } 
    }
}