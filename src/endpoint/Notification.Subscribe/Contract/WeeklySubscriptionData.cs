namespace GarageGroup.Internal.Timesheet;

public record class WeeklySubscriptionData : BaseSubscriptionData
{
    public WeeklySubscriptionData(WeeklyTimesheetNotificationUserPreference userPreference)
        => 
        UserPreference = userPreference;
    
    public override NotificationType NotificationType { get; } = NotificationType.WeeklyNotification;
    
    public override WeeklyTimesheetNotificationUserPreference? UserPreference { get; }
}

public sealed record class WeeklyTimesheetNotificationUserPreference : INotificationUserPreference
{
    public int Weekday { get; init; }
    
    public TimeSpan FlowRuntime { get; init; }
    
    public int WorkedHours { get; init; }
}