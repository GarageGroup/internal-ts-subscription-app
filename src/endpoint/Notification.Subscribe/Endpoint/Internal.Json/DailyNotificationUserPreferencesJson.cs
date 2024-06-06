using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class DailyNotificationUserPreferencesJson
{
    public static DailyNotificationUserPreferencesJson Parse(DailyNotificationUserPreference userPreference)
    {
        ArgumentNullException.ThrowIfNull(userPreference);
        
        return new()
        {
            WorkedHours = userPreference.WorkedHours,
            FlowRuntime = userPreference.FlowRuntime
        };
    }
    
    public int WorkedHours { get; init; }
    
    public TimeOnly FlowRuntime { get; init; }
}