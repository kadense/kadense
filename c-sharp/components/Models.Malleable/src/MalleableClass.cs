namespace Kadense.Models.Malleable
{
    public class MalleableClass
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
        /// The discriminator property for this class
        /// </summary>
        [JsonPropertyName("discriminatorProperty")]
        public string? DiscriminatorProperty { get; set; }

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

    }
}