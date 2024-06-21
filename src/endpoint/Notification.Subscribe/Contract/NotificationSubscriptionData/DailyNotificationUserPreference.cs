using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

public sealed record class DailyNotificationUserPreference : INotificationUserPreference
{
    public DailyNotificationUserPreference(int workedHours, [AllowNull] NotificationTime notificationTime)
    {
        WorkedHours = workedHours;
        NotificationTime = notificationTime ?? NotificationTime.Msk18;
    }

    [SwaggerDescription(In.DailyNotificationWorkedHoursDescription)]
    [IntegerExample(In.DailyNotificationWorkedHoursExample)]
    public int WorkedHours { get; }

    [SwaggerDescription(In.NotificationTimeDescription)]
    public NotificationTime NotificationTime { get; }
}