using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class Subscription
{
    public Guid Id { get; init; } 
    
    public NotificationType NotificationType { get; init; }

    public INotificationUserPreference? UserPreference { get; init; }

    public bool IsActive { get; init; }
}