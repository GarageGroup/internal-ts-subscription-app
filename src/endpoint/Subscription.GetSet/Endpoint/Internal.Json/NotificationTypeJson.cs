using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class NotificationTypeJson
{
    internal const string KeyFieldName = "gg_key";

    [JsonPropertyName(KeyFieldName)]
    public string? Key { get; init; }
}