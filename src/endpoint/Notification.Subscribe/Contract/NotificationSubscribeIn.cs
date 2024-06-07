using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class NotificationSubscribeIn
{
    public NotificationSubscribeIn(
        [ClaimIn] Guid systemUserId,
        [RootBodyIn] BaseSubscriptionData subscriptionData)
    {
        SystemUserId = systemUserId;
        SubscriptionData = subscriptionData;
    }
    
    public Guid SystemUserId { get; }

    public BaseSubscriptionData SubscriptionData { get; }
}