using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class WeeklyNotificationUserPreference : INotificationUserPreference
{
    public WeeklyNotificationUserPreference(FlatArray<Weekday> weekday, int workedHours, [AllowNull] NotificationTime notificationTime)
    {
        Weekday = weekday;
        WorkedHours = workedHours;
        NotificationTime = notificationTime ?? NotificationTime.Msk18;
    }

    [StringExample(nameof(Timesheet.Weekday.Friday))]
    public FlatArray<Weekday> Weekday { get; }

    [IntegerExample(40)]
    public int WorkedHours { get; }

    public NotificationTime NotificationTime { get; }
}