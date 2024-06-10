using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class DailyNotificationUserPreference : INotificationUserPreference
{
    public DailyNotificationUserPreference(int workedHours, [AllowNull] NotificationTime notificationTime)
    {
        WorkedHours = workedHours;
        NotificationTime = notificationTime ?? NotificationTime.Msk18;
    }

    [IntegerExample(8)]
    public int WorkedHours { get; }

    public NotificationTime NotificationTime { get; }
}