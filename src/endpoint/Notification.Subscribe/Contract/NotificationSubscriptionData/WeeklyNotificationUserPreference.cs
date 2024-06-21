using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

public sealed record class WeeklyNotificationUserPreference : INotificationUserPreference
{
    public WeeklyNotificationUserPreference(FlatArray<Weekday> weekday, int workedHours, [AllowNull] NotificationTime notificationTime)
    {
        Weekday = weekday;
        WorkedHours = workedHours;
        NotificationTime = notificationTime ?? NotificationTime.Msk18;
    }

    [SwaggerDescription(In.WeeklyNotificationWeekdayDescription)]
    [StringExample(In.WeeklyNotificationWeekdayExample)]
    public FlatArray<Weekday> Weekday { get; }

    [SwaggerDescription(In.WeeklyNotificationWorkedHoursDescription)]
    [IntegerExample(In.WeeklyNotificationWorkedHoursExample)]
    public int WorkedHours { get; }

    [SwaggerDescription(In.NotificationTimeDescription)]
    public NotificationTime NotificationTime { get; }
}