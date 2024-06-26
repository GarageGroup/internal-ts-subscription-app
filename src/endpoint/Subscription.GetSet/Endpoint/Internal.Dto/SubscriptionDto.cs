using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class SubscriptionDto
{
    public Guid Id { get; init; }
    
    public NotificationType NotificationType { get; init; }
    
    public INotificationUserPreferenceDto? UserPreference { get; init; }
}