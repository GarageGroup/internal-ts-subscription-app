using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

public sealed record class NotificationUnsubscribeIn
{
    public NotificationUnsubscribeIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn, SwaggerDescription(In.NotificationTypeDesciption)] NotificationType notificationType)
    {
        SystemUserId = systemUserId;
        NotificationType = notificationType;
    }

    public Guid SystemUserId { get; }

    public NotificationType NotificationType { get; }
}