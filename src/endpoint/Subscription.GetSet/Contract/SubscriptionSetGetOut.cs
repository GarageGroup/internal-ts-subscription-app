using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class SubscriptionSetGetOut
{
    [JsonBodyOut]
    public FlatArray<Subscription> Subscriptions { get; init; }
}