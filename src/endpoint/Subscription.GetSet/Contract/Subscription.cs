using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class Subscription
{
    [SwaggerDescription(Out.IdDescription)]
    [StringExample(Out.IdExample)]
    public Guid Id { get; init; } 
    
    [SwaggerDescription(Out.NotificationTypeDescription)]
    [StringExample(Out.NotificationTypeExample)]
    public NotificationType NotificationType { get; init; }

    [SwaggerDescription(Out.UserPreferenceDescription)]
    public INotificationUserPreference? UserPreference { get; init; }
}