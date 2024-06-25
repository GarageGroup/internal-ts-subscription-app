using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed class FlowRuntimeConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
        {
            throw new JsonException($"JSON token type must be string");
        }

        var flowRuntime = reader.GetString();
        if (string.IsNullOrEmpty(flowRuntime))
        {
            throw new JsonException($"Flow runtime is not specified");
        }

        return TimeOnly.Parse(flowRuntime);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("T", CultureInfo.InvariantCulture));
    }
}