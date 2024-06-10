using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class NotificationUnsubscribeIn
{
    public NotificationUnsubscribeIn(
        [ClaimIn] Guid systemUserId,
        [RootBodyIn] NotificationType notificationType)
    {
        SystemUserId = systemUserId;
        NotificationType = notificationType;
    }
    
    public Guid SystemUserId { get; }

    public NotificationType NotificationType { get; }
}