namespace Kadense.Models.Malleable
{
    public class MalleableProperty : ICloneable
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "string";

        [JsonPropertyName("subType")]
        public string? SubType { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("propertyEnum")]
        public List<string>? PropertyEnum { get; set; }

        [JsonPropertyName("default")]
        public string? Default { get; set; }

        [JsonPropertyName("nullable")]
        public bool? Nullable { get; set; }

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
            if (string.IsNullOrEmpty(Type))
            {
                return true; // When Type is null or empty, we assume it's a string
            }

            var primitiveTypes = new List<string> { "string", "int", "long", "double", "bool", "short", "char", "byte", "ulong", "sbyte", "float", "decimal", "datetime", "datetimeoffset", "timespan" };
            return primitiveTypes.Contains(Type.ToLower());
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

        public bool IsComplexType()
        {
            if (IsPrimitiveType())
                return false; // Primitive types are not complex

            if (IsCollectionType() && IsPrimitiveSubType())
                return false; // Collection of primitive types is not complex

            if (IsDictionaryType() && IsPrimitiveSubType())
                return false; // Dictionary of primitive types is not complex

            return true; // If it's not primitive and not a collection or a dictionary of primitive types, it's complex
        }

        public bool IsCollectionType()
        {
            if (Type == null)
            {
                return false;
            }

            return Type == "list" || Type == "array";
        }

        public bool IsDictionaryType()
        {
            if (Type == null)
            {
                return false;
            }

            return Type == "dictionary" || Type == "map";
        }

        public object Clone()
        {
            return new MalleableProperty
            {
                Description = Description,
                Type = Type,
                SubType = SubType,
                Format = Format,
                PropertyEnum = PropertyEnum?.ToList(),
                Default = Default,
                Nullable = Nullable,
                Categories = Categories.ToList()
            };
        }

        public (bool, MalleableTypeReference?) TryGetComplexTypeReference(string? defaultNamespace = null, string? defaultModule = null)
        {
            if (!this.IsComplexType())
                return (false, null);

            if (this.IsDictionaryType() || this.IsCollectionType())
            {
                var (_, subTypeRef) = TryGetSubTypeReference(defaultNamespace, defaultModule);
                return (true, subTypeRef);
            }

            var (_, typeRef) = TryGetTypeReference(defaultNamespace, defaultModule);
            return (true, typeRef);
        }

        public (bool, MalleableTypeReference?) TryGetTypeReference(string? defaultNamespace = null, string? defaultModule = null)
        {
            if (string.IsNullOrEmpty(Type))
                return (false, null);

            var typeReference = new MalleableTypeReference();
            if (Type.Contains(":"))
            {
                var parts = Type.Split(':');
                typeReference.ModuleNamespace = parts[0];
                typeReference.ModuleName = parts[1];
                typeReference.ClassName = parts[2];
                return (true, typeReference);
            }
            else
            {
                typeReference.ClassName = Type;
                typeReference.ModuleNamespace = defaultNamespace;
                typeReference.ModuleName = defaultModule;
                return (false, typeReference);
            }
        }
        public (bool, MalleableTypeReference?) TryGetSubTypeReference(string? defaultNamespace = null, string? defaultModule = null)
        {
            if (string.IsNullOrEmpty(SubType))
                return (false, null);

            var typeReference = new MalleableTypeReference();
            if (SubType!.Contains(":"))
            {
                var parts = SubType.Split(':');
                typeReference.ModuleNamespace = parts[0];
                typeReference.ModuleName = parts[1];
                typeReference.ClassName = parts[2];
                return (true, typeReference);
            }
            else
            {
                typeReference.ClassName = SubType;
                typeReference.ModuleNamespace = defaultNamespace;
                typeReference.ModuleName = defaultModule;
                return (false, typeReference);
            }
        }


        public override string ToString()
        {
            if(IsPrimitiveType())
            {
                return Type;
            }
            else if (IsCollectionType())
            {
                return $"List<{SubType}>";
            }
            else if (IsDictionaryType())
            {
                return $"Dictionary<string, {SubType}>";
            }
            else
            {
                return Type;
            }
        }
    }
}