using System;

namespace GarageGroup.Internal.Timesheet;

public record class DailySubscriptionData : BaseSubscriptionData
{
    public DailySubscriptionData(DailyTimesheetNotificationUserPreference userPreference)
        => 
        UserPreference = userPreference;
    
    public override NotificationType NotificationType { get; } = NotificationType.DailyNotification;

    public override DailyTimesheetNotificationUserPreference? UserPreference { get; }
}

public sealed record class DailyTimesheetNotificationUserPreference : INotificationUserPreference
{
    public int WorkedHours { get; init; }
    
    public TimeSpan FlowRuntime { get; init; }
}