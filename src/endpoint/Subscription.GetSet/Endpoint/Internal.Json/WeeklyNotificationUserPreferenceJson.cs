using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class WeeklyNotificationUserPreferenceJson
{
    public string? Weekday { get; init; }

    public decimal WorkedHours { get; init; }

    [JsonPropertyName("flowRuntime")]
    public string? NotificationTime { get; init; }
}