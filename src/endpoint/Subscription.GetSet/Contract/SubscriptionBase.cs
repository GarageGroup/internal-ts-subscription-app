using System.Text.Json.Serialization;
using GarageGroup.Infra;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

[JsonDerivedType(typeof(DailyNotificationSubscription))]
[JsonDerivedType(typeof(WeeklyNotificationSubscription))]
public abstract record class SubscriptionBase : IOpenApiSchemaProvider
{
    public static OpenApiSchema GetSchema(bool nullable, IOpenApiAny? example = null, string? description = null)
        =>
        new()
        {
            Description = description,
            AnyOf =
            [
                DailyNotificationSubscription.Schema,
                WeeklyNotificationSubscription.Schema
            ]
        };

    [SwaggerDescription(Out.NotificationTypeDescription)]
    public abstract NotificationType NotificationType { get; }

    [SwaggerDescription(Out.UserPreferenceDescription)]
    public abstract INotificationUserPreference? UserPreference { get; }
}