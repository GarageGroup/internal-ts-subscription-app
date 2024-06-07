using System;
using System.Linq;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class WeeklyNotificationUserPreferencesJson
{
    internal static WeeklyNotificationUserPreferencesJson Parse(WeeklyNotificationUserPreference userPreference)
    {
        return new()
        {
            Weekday = string.Join(',', userPreference.Weekday.AsEnumerable().Select(AsInt32)),
            FlowRuntime = userPreference.FlowRuntime,
            WorkedHours = userPreference.WorkedHours
        };

        static int AsInt32(Weekday weekday)
            =>
            (int)weekday;
    }

    public string? Weekday { get; init; }

    public int WorkedHours { get; init; }

    public TimeOnly FlowRuntime { get; init; }
}