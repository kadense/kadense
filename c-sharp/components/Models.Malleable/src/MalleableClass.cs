using System.Reflection.Metadata;

namespace Kadense.Models.Malleable
{
    public class MalleableClass : ICloneable
    {
        /// <summary>
        /// The base class which this class is derived from
        /// </summary>
        [JsonPropertyName("baseClass")]
        public string? BaseClass { get; set; }
        /// <summary>
        /// The description for this class
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The properties for this class
        /// </summary>
        [JsonPropertyName("properties")]
        public Dictionary<string, MalleableProperty>? Properties { get; set; }

        /// <summary>
        /// The class which this class is derived from, if blank it will use the base class
        /// </summary>
        [JsonPropertyName("discriminatorClass")]
        public string? DiscriminatorClass { get; set; }
        /// <summary>
        /// The discriminator property for this class
        /// </summary>
        [JsonPropertyName("discriminatorProperty")]
        public string? DiscriminatorProperty { get; set; }

        /// <summary>
        /// The discriminator value for this class
        /// </summary>
        [JsonPropertyName("typeDiscriminator")]
        public string? TypeDiscriminator { get; set; }

        /// <summary>
        /// An expression that can be used to uniquely identify this class
        /// </summary>
        [JsonPropertyName("identifierExpression")]
        public string? IdentifierExpression { get; set; }

        /// <summary>
        /// The module parameters
        /// </summary>
        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = new List<string>() { "Public" };

        public object Clone()
        {
            var clone = new MalleableClass();
            clone.BaseClass = BaseClass;
            clone.Description = Description;
            clone.Properties = Properties?.ToDictionary(x => x.Key, x => (MalleableProperty)x.Value.Clone());
            clone.DiscriminatorClass = DiscriminatorClass;
            clone.DiscriminatorProperty = DiscriminatorProperty;
            clone.TypeDiscriminator = TypeDiscriminator;
            clone.IdentifierExpression = IdentifierExpression;
            clone.Categories = Categories.ToList();
            return clone;
        }

        public bool MatchesCategories(string[] categories)
        {
            bool matchedProperty = false;
            foreach (var category in categories)
            {
                if (Categories.Contains(category, StringComparer.CurrentCultureIgnoreCase))
                {
                    matchedProperty = true;
                    break;
                }
            }
            return matchedProperty;
        }

        public (bool, MalleableTypeReference?) TryGetBaseClassReference(string? defaultNamespace = null, string? defaultModule = null)
        {
            if (string.IsNullOrEmpty(BaseClass))
                return (false, null);

            var typeReference = new MalleableTypeReference();
            if (BaseClass.Contains(":"))
            {
                var parts = BaseClass.Split(':');
                typeReference.ModuleNamespace = parts[0];
                typeReference.ModuleName = parts[1];
                typeReference.ClassName = parts[2];
                return (true, typeReference);
            }
            else
            {
                typeReference.ClassName = BaseClass;
                typeReference.ModuleNamespace = defaultNamespace;
                typeReference.ModuleName = defaultModule;
                return (false, typeReference);
            }
        }

        public (bool, MalleableTypeReference?) TryGetDiscriminatorClassReference(string? defaultNamespace = null, string? defaultModule = null)
        {
            if (string.IsNullOrEmpty(DiscriminatorClass))
                return (false, null);

            var typeReference = new MalleableTypeReference();
            if (DiscriminatorClass.Contains(":"))
            {
                var parts = DiscriminatorClass.Split(':');
                typeReference.ModuleNamespace = parts[0];
                typeReference.ModuleName = parts[1];
                typeReference.ClassName = parts[2];
                return (true, typeReference);
            }
            else
            {
                typeReference.ClassName = DiscriminatorClass;
                typeReference.ModuleNamespace = defaultNamespace;
                typeReference.ModuleName = defaultModule;
                return (false, typeReference);
            }
        }

        public IList<MalleableTypeReference> GetReferencedTypes()
        {
            var list = new List<MalleableTypeReference>();
            if (Properties == null)
                return list;

            foreach (var property in Properties)
            {
                var (isReference, typeRef) = property.Value.TryGetTypeReference();
                if (isReference && typeRef != null)
                    list.Add(typeRef);

                var (isSubTypeReference, subTypeRef) = property.Value.TryGetSubTypeReference();
                if (isSubTypeReference && subTypeRef != null)
                    list.Add(subTypeRef);
            }
            return list;
        }
    }
}