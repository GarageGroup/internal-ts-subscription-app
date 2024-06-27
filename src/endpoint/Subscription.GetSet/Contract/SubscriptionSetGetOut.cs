using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public readonly record struct SubscriptionSetGetOut
{
    [JsonBodyOut, SwaggerDescription(Out.SubscriptionsDescription)]
    public FlatArray<SubscriptionBase> Subscriptions { get; init; }
}