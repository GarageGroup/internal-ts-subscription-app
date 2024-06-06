namespace GarageGroup.Internal.Timesheet;

public sealed record class WeeklyNotificationSubscriptionData : BaseSubscriptionData
{
    public WeeklyNotificationSubscriptionData(WeeklyNotificationUserPreference userPreference)
        => 
        UserPreference = userPreference;
    
    public override NotificationType NotificationType { get; } = NotificationType.WeeklyNotification;
    
    public override WeeklyNotificationUserPreference? UserPreference { get; }
}