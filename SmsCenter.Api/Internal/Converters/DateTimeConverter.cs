using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmsCenter.Api.Internal.Converters;

internal class DateTimeConverter : JsonConverter<DateTime>
{
    private DateTimeConverter() { }
    public static readonly DateTimeConverter Instance = new();

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString() ?? throw new InvalidOperationException();
        return DateTime.Parse(str);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd.MM.yyyy hh:mm:ss"));
    }
}