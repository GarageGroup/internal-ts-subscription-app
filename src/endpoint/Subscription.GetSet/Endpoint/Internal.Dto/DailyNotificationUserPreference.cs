using System;
using System.Text.Json.Serialization;
using GarageGroup.Internal.Timesheet;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class DailyNotificationUserPreferenceDto : INotificationUserPreferenceDto
{
    public int WorkedHours { get; init; }

    [JsonPropertyName("flowRuntime")]
    [JsonConverter(typeof(FlowRuntimeConverter))]
    public TimeOnly NotificationTime { get; init; }
}