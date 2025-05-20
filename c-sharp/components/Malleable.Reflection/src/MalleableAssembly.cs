using System.Text.Json.Serialization.Metadata;

namespace Kadense.Malleable.Reflection
{
    public class MalleableAssembly
    {
        public string Name { get; set; }

        public MalleableAssembly(string name)
        {
            Name = name;
        }

        public IDictionary<string, Type> Types { get; private set; } = new Dictionary<string, Type>();
        public IDictionary<string, Type> ExpressionParameters { get; private set; } = new Dictionary<string, Type>();

        public void AddType(string name, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!Types.ContainsKey(name))
                Types.Add(name, type);
        }

        public IDictionary<Type, MalleableJsonPolymorphicOptions> JsonPolymorphicOptions { get; } = new Dictionary<Type, MalleableJsonPolymorphicOptions>();
    }
}