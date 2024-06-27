using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class DailyNotificationUserPreferenceJson
{
    public decimal WorkedHours { get; init; }

    [JsonPropertyName("flowRuntime")]
    public string? NotificationTime { get; init; }
}