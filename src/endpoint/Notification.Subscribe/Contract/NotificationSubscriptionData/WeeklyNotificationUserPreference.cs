using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class WeeklyNotificationUserPreference : INotificationUserPreference
{
    public FlatArray<Weekday> Weekday { get; init; }
    
    public TimeOnly FlowRuntime { get; init; }

    public int WorkedHours { get; init; }
}