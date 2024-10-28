using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmsCenter.Api.Internal.Converters;

public class StringConverter : JsonConverter<string>
{
    private StringConverter()
    {
    }

    public static readonly StringConverter Instance = new();

    // Читать пустую строку как null
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null) return null;
        return string.IsNullOrEmpty(value) ? null : value;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}