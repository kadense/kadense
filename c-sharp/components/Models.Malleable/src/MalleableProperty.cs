namespace Kadense.Models.Malleable
{
    public class MalleableProperty
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public string PropertyType { get; set; } = "string";

        [JsonPropertyName("subType")]
        public string? SubType { get;set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("propertyEnum")]
        public List<string>? PropertyEnum { get; set; }

        [JsonPropertyName("default")]
        public string? Default { get; set; }

        [JsonPropertyName("nullable")]
        public bool Nullable { get; set; } = true;

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

        public bool IsPrimitiveType()
        {
            if (string.IsNullOrEmpty(PropertyType))
            {
                return true; // When PropertyType is null or empty, we assume it's a string
            }

            var primitiveTypes = new List<string> { "string", "int", "long", "double", "bool", "short", "char", "byte", "ulong", "sbyte", "float", "decimal", "datetime", "datetimeoffset", "timespan" };
            return primitiveTypes.Contains(PropertyType.ToLower());
        }

        public bool IsPrimitiveSubType()
        {
            if (string.IsNullOrEmpty(SubType))
            {
                return true; // When SubType is null or empty, we assume it's a string
            }
            var primitiveTypes = new List<string> { "string", "int", "long", "double", "bool", "short", "char", "byte", "ulong", "sbyte", "float", "decimal", "datetime", "datetimeoffset", "timespan" };
            return primitiveTypes.Contains(SubType.ToLower());
        }

        public bool IsCollectionType()
        {
            if(PropertyType == null)
            {
                return false;
            }
            
            return PropertyType == "list" || PropertyType == "array";
        }

        public bool IsDictionaryType()
        {
            if(PropertyType == null)
            {
                return false;
            }
            
            return PropertyType == "dictionary" || PropertyType == "map";
        }
    }
}