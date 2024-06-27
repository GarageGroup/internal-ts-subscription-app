using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class SubscriptionSetGetIn
{
    public SubscriptionSetGetIn([ClaimIn] Guid systemUserId)
        => 
        SystemUserId = systemUserId;
    
    public Guid SystemUserId { get; }
}