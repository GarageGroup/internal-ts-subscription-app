using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class WeeklyNotificationUserPreferenceDtoDto : INotificationUserPreferenceDto
{
    [JsonConverter(typeof(WeekdaysConverter))]
    public FlatArray<Weekday> Weekday { get; init; }

    public int WorkedHours { get; init; }

    [JsonPropertyName("flowRuntime")]
    [JsonConverter(typeof(FlowRuntimeConverter))]
    public TimeOnly NotificationTime { get; init; }
    
    private sealed class WeekdaysConverter : JsonConverter<FlatArray<Weekday>>
    {
        public override FlatArray<Weekday> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return FlatArray<Weekday>.Empty;
            }

            if (reader.TokenType is not JsonTokenType.String)
            {
                throw new JsonException($"JSON token type must be string");
            }

            var text = reader.GetString();
            if (string.IsNullOrEmpty(text))
            {
                return FlatArray<Weekday>.Empty;
            }
        
            return text
                .Split(",")
                .ToFlatArray()
                .Map(Enum.Parse<Weekday>);
        }

        public override void Write(Utf8JsonWriter writer, FlatArray<Weekday> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var weekday in value)
            {
                writer.WriteStringValue(weekday.ToString());
            }
        
            writer.WriteEndArray();
        }
    }
}