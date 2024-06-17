using System.Collections.Generic;
using System.Text.Json;

namespace GarageGroup.Internal.Timesheet;

internal sealed class JsonObject : Dictionary<string, JsonElement>
{
    public void AddProperty(string propertyName, object value)
    {
        if (value == null)
        {
            Add(propertyName, default);
        }
        else
        {
            var jsonValue = JsonSerializer.SerializeToUtf8Bytes(value);
            using var document = JsonDocument.Parse(jsonValue);
            Add(propertyName, document.RootElement.Clone());
        }
    }

    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}