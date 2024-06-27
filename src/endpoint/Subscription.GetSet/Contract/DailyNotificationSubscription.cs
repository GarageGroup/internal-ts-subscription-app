using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class DailyNotificationSubscription : SubscriptionBase
{
    public static OpenApiSchema Schema { get; }
        =
        new()
        {
            Title = "Daily notification subscription",
            Description = "Daily notification subscription",
            Example = new OpenApiObject
            {
                [NamingPolicy.ConvertName(nameof(NotificationType))] = new OpenApiString(nameof(NotificationType.DailyNotification)),
                [NamingPolicy.ConvertName(nameof(UserPreference))] = DailyNotificationUserPreference.Example
            }
        };

    public DailyNotificationSubscription(DailyNotificationUserPreference? userPreference)
        =>
        UserPreference = userPreference;

    public override NotificationType NotificationType { get; } = NotificationType.DailyNotification;

    public override DailyNotificationUserPreference? UserPreference { get; }
}