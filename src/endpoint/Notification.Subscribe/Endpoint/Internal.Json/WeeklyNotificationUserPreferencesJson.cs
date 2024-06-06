using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class WeeklyNotificationUserPreferencesJson
{    
    public static WeeklyNotificationUserPreferencesJson Parse(WeeklyNotificationUserPreference userPreference)
    {
        ArgumentNullException.ThrowIfNull(userPreference);

        return new WeeklyNotificationUserPreferencesJson
        {
            Weekday = string.Join(", ", userPreference.Weekday.Map(x => (int)x).ToArray()),
            FlowRuntime = userPreference.FlowRuntime,
            WorkedHours = userPreference.WorkedHours
        };
    }
    
    public string? Weekday { get; init; }
    
    public TimeOnly FlowRuntime { get; init; }

    public int WorkedHours { get; init; }
    
}