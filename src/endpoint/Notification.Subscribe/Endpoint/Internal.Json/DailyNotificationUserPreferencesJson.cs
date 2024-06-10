using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class DailyNotificationUserPreferencesJson
{
    internal static DailyNotificationUserPreferencesJson Parse(DailyNotificationUserPreference userPreference)
        =>
        new()
        {
            WorkedHours = userPreference.WorkedHours,
            FlowRuntime = userPreference.FlowRuntime
        };

    public int WorkedHours { get; init; }

    public TimeOnly FlowRuntime { get; init; }
}
