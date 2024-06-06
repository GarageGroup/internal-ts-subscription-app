namespace GarageGroup.Internal.Timesheet;

public sealed record class DailyNotificationSubscriptionData : BaseSubscriptionData
{
    public DailyNotificationSubscriptionData(DailyNotificationUserPreference userPreference)
        => 
        UserPreference = userPreference;
    
    public override NotificationType NotificationType { get; } = NotificationType.DailyNotification;

    public override DailyNotificationUserPreference? UserPreference { get; }
}