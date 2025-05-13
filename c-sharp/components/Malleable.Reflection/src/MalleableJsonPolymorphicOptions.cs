
using System.Text.Json.Serialization.Metadata;

namespace Kadense.Malleable.Reflection
{
    public class MalleableJsonPolymorphicOptions
    {
        public string TypeDiscriminatorPropertyName { get; set; } = "$type";
        public IList<JsonDerivedType> DerivedTypes { get; set; } = new List<JsonDerivedType>();
    }

}