using System.Text.Json;

namespace Kadense.Models.Malleable;

public class NumberOrStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch(reader.TokenType)
        {
            case JsonTokenType.Number:
                if (reader.TryGetInt32(out int intValue))
                {
                    return intValue.ToString();
                }
                else if (reader.TryGetInt64(out long longValue))
                {
                    return longValue.ToString();
                }
                break;
            case JsonTokenType.String:
                return reader.GetString();
        }
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType());
    }
}