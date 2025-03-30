using System.Text.Json.Serialization;
using System.Reflection;

namespace Kadense.Models.Kubernetes
{
    /// <summary>
    /// A builder class for generating Kubernetes CustomResourceDefinition (CRD) YAML files.
    /// </summary>
    public class CustomResourceDefinitionBuilder
    {
        /// <summary>
        /// Builds a CRD YAML file for a given type and writes it to the provided stream.
        /// </summary>
        /// <param name="stream">The stream to write the CRD YAML to.</param>
        /// <param name="type">The type representing the custom resource.</param>
        public static async Task BuildAsync(Stream stream, Type type)
        {
            // Initialize a StreamWriter to write to the stream.
            var writer = new StreamWriter(stream);

            // Extract custom resource attributes from the type.
            var crdHeaders = (KubernetesCustomResourceAttribute)type.GetCustomAttributes(typeof(KubernetesCustomResourceAttribute), false).First();
            var properties = type.GetProperties();

            // Write the CRD YAML structure.
            await writer.WriteLineAsync("apiVersion: apiextensions.k8s.io/v1");
            await writer.WriteLineAsync("kind: CustomResourceDefinition");
            await writer.WriteLineAsync("metadata:");
            await PadAsync(writer, 1);
            await writer.WriteLineAsync($"name: {crdHeaders.PluralName.ToLower()}.{crdHeaders.Group}");
            await writer.WriteLineAsync("spec:");
            
            await PadAsync(writer, 1);
            await writer.WriteLineAsync($"scope: Namespaced");

            await PadAsync(writer, 1);
            await writer.WriteLineAsync($"group: {crdHeaders.Group}");

            await PadAsync(writer, 1);
            await writer.WriteLineAsync($"names:");
            await PadAsync(writer, 2);
            await writer.WriteLineAsync($"kind: {crdHeaders.Kind}");
            await PadAsync(writer, 2);
            await writer.WriteLineAsync($"plural: {crdHeaders.PluralName.ToLower()}");
            await PadAsync(writer, 2);
            await writer.WriteLineAsync($"singular: {crdHeaders.Kind.ToLower()}");



            await PadAsync(writer, 1);
            await writer.WriteLineAsync($"versions:");

            await PadAsync(writer, 1);
            await writer.WriteLineAsync($"- name: {crdHeaders.Version}");

            await PadAsync(writer, 2);
            await writer.WriteLineAsync($"served: true");

            await PadAsync(writer, 2);
            await writer.WriteLineAsync($"storage: true");

            await PadAsync(writer, 2);
            await writer.WriteLineAsync($"schema:");

            await PadAsync(writer, 3);
            await writer.WriteLineAsync($"openAPIV3Schema:");

            await PadAsync(writer, 4);
            await writer.WriteLineAsync($"type: object");

            await PadAsync(writer, 4);
            await writer.WriteLineAsync($"properties:");

            await PadAsync(writer, 5);
            await writer.WriteLineAsync($"spec:");

            await PadAsync(writer, 6);
            await writer.WriteLineAsync($"type: object");

            await PadAsync(writer, 6);
            await writer.WriteLineAsync($"properties:");

            await ProcessClassAsync(writer, type, 7);

            // Flush the writer and reset the stream position.
            await writer.FlushAsync();
            stream.Position = 0;
        }

        /// <summary>
        /// Processes the properties of a class and writes their schema definitions to the CRD YAML.
        /// </summary>
        /// <param name="writer">The StreamWriter to write the YAML.</param>
        /// <param name="type">The type of the class to process.</param>
        /// <param name="depth">The indentation depth for YAML formatting.</param>
        private static async Task ProcessClassAsync(StreamWriter writer, Type type, int depth)
        {
            foreach (var property in type.GetProperties())
            {
                // Write the property name.
                await PadAsync(writer, depth);
                var jsonPropNames = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false);
                if (jsonPropNames.Length > 0)
                {
                    await writer.WriteAsync(((JsonPropertyNameAttribute)jsonPropNames.First()).Name);
                }
                else
                {
                    string propertyName = property.Name;
                    if (propertyName.Length > 1)
                    {
                        propertyName = $"{propertyName.Substring(0, 1).ToLower()}{propertyName.Substring(1)}";
                    }
                    await writer.WriteAsync(propertyName);
                }
                await writer.WriteLineAsync(":");

                // Determine the property type and write its schema.
                await ProcessPropertyAsync(writer, property, depth);
            }
        }

        private static async Task ProcessPropertyAsync(StreamWriter writer, PropertyInfo property, int depth)
        {
            Type propertyType = property.PropertyType;

            if (property.PropertyType.Equals(typeof(string)))
            {
                await PadAsync(writer, depth + 1);
                await writer.WriteLineAsync("type: string");
            }
            else if (property.PropertyType.Equals(typeof(int)))
            {
                await PadAsync(writer, depth + 1);
                await writer.WriteLineAsync("type: integer");
            }
            else if (property.PropertyType.IsEnum)
            {
                await PadAsync(writer, depth + 1);
                await writer.WriteLineAsync("type: string");
                await PadAsync(writer, depth + 1);
                await writer.WriteLineAsync("enum:");
                foreach (string? enumName in property.PropertyType.GetEnumNames())
                {
                    await PadAsync(writer, depth + 1);
                    await writer.WriteLineAsync($"- {enumName}");
                }
            }
            else if (property.PropertyType.IsArray)
            {
                // Array types are not yet implemented.
                throw new NotImplementedException();
            }
            else if (property.PropertyType.IsGenericType)
            {
                Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>)) 
                {
                    await PadAsync(writer, depth + 1);
                    await writer.WriteLineAsync("type: object");

                    await PadAsync(writer, depth + 1);
                    await writer.WriteLineAsync("additionalProperties:");

                    if (property.PropertyType.GetGenericArguments()[0] != typeof(string))
                        throw new NotImplementedException();

                    if (property.PropertyType.GetGenericArguments()[1] == typeof(string))
                    {
                        await PadAsync(writer, depth + 2);
                        await writer.WriteLineAsync("type: string");
                    }
                    else
                    {
                        await PadAsync(writer, depth + 2);
                        await writer.WriteLineAsync("type: object");
                        await PadAsync(writer, depth + 2);
                        await writer.WriteLineAsync("properties:");
                        await ProcessClassAsync(writer, property.PropertyType, depth + 3);
                    }
                }
                else if(genericTypeDefinition == typeof(List<>))
                {
                    await PadAsync(writer, depth + 1);
                    await writer.WriteLineAsync("type: array");

                    await PadAsync(writer, depth + 1);
                    await writer.WriteLineAsync("items:");

                    if (property.PropertyType.GetGenericArguments()[0] == typeof(string))
                    {
                        await PadAsync(writer, depth + 2);
                        await writer.WriteLineAsync("type: string");
                    }
                    else
                    {
                        await PadAsync(writer, depth + 2);
                        await writer.WriteLineAsync("type: object");
                        await PadAsync(writer, depth + 2);
                        await writer.WriteLineAsync("properties:");
                        await ProcessClassAsync(writer, property.PropertyType, depth + 3);
                    }
                }
                else if(genericTypeDefinition == typeof(Nullable<>))
                {
                    
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                // For complex types, recursively process their properties.
                await PadAsync(writer, depth + 1);
                await writer.WriteLineAsync("type: object");
                await PadAsync(writer, depth + 1);
                await writer.WriteLineAsync("properties:");
                await ProcessClassAsync(writer, property.PropertyType, depth + 2);
            }
        }

        /// <summary>
        /// Writes indentation spaces to the StreamWriter for YAML formatting.
        /// </summary>
        /// <param name="writer">The StreamWriter to write the spaces.</param>
        /// <param name="depth">The number of indentation levels.</param>
        private static async Task PadAsync(StreamWriter writer, int depth)
        {
            for (int i = 0; i < depth; i++)
                await writer.WriteAsync("  ");
        }
    }
}