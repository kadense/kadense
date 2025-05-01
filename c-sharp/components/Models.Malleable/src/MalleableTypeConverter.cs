namespace Kadense.Models.Malleable
{
    public class MalleableTypeConverter
    {
        /// <summary>
        /// The type of object to convert from
        /// </summary>
        [JsonPropertyName("from")]
        public MalleableTypeReference? From { get; set; }

        /// <summary>
        /// The type of object to convert to
        /// </summary>
        [JsonPropertyName("to")]
        public MalleableTypeReference? To { get; set; }

        /// <summary>
        /// When not null it will change the input property to return the result of the expression.
        /// </summary>
        [JsonPropertyName("inputExpression")]
        public string? InputExpression { get; set; }


        /// <summary>
        /// Categories which are supported by this converter
        /// </summary>
        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = new List<string>() { "Public" };

        /// <summary>
        /// Expressions used to populate the properties on the destination object
        /// </summary>
        [JsonPropertyName("expressions")]
        public Dictionary<string, string> Expressions { get; set; } = new Dictionary<string, string>();

        
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