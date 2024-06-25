using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class DailyNotificationUserPreference : INotificationUserPreference
{
    public int WorkedHours { get; init; }

    public TimeOnly NotificationTime { get; init; }
}