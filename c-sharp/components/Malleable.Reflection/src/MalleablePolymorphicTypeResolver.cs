using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Kadense.Malleable.Reflection
{
    public class MalleablePolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public IList<MalleableAssembly> MalleableAssembly { get; } = new List<MalleableAssembly>();
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);


            foreach(var assembly in MalleableAssembly)
            {
                foreach(var types in assembly.JsonPolymorphicOptions)
                {
                    if (jsonTypeInfo.Type == types.Key)
                    {
                        jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = types.Value.TypeDiscriminatorPropertyName,
                            IgnoreUnrecognizedTypeDiscriminators = true,
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                        };
                        foreach(var derivedType in types.Value.DerivedTypes)
                        {
                            jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
                        }
                    }
                }
            }

            return jsonTypeInfo;
        }
    }
}