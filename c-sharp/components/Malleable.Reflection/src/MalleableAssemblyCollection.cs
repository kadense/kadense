using System.Text.Json.Serialization.Metadata;

namespace Kadense.Malleable.Reflection
{
    public class MalleableAssemblyCollection : Dictionary<string, MalleableAssembly>
    {
        public Type GetType(string qualifierModuleName, string typeName)
        {
            if (TryGetValue(qualifierModuleName, out var assembly))
            {
                if (assembly.Types.TryGetValue(typeName, out var type))
                {
                    return type;
                }
            }
            throw new KeyNotFoundException($"Type '{typeName}' not found in assembly '{qualifierModuleName}'.");
        }

        public Type GetType(string typeName)
        {
            var types = new Dictionary<string, Type>();
            foreach (var assembly in this)
            {
                assembly.Value.Types.TryGetValue(typeName, out var type);
                if (type != null)
                    types.Add(assembly.Key, type);
            }

            if(types.Count == 1)
            {
                return types.First().Value;
            }
            else if (types.Count > 1)
            {
                throw new InvalidOperationException($"Type '{typeName}' is ambiguous across multiple assemblies: {string.Join(", ", types.Keys)}.");
            }
            else
            {
                throw new KeyNotFoundException($"Type '{typeName}' not found in any assembly.");
            }
        }


    }
}