using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class WeeklyNotificationSubscription : SubscriptionBase
{
    public static OpenApiSchema Schema { get; }
        =
        new()
        {
            Title = "Weekly notification subscription",
            Description = "Weekly notification subscription",
            Example = new OpenApiObject
            {
                [NamingPolicy.ConvertName(nameof(NotificationType))] = new OpenApiString(nameof(NotificationType.WeeklyNotification)),
                [NamingPolicy.ConvertName(nameof(UserPreference))] = WeeklyNotificationUserPreference.Example
            }
        };

    public WeeklyNotificationSubscription(WeeklyNotificationUserPreference? userPreference)
        =>
        UserPreference = userPreference;

    public override NotificationType NotificationType { get; } = NotificationType.WeeklyNotification;

    public override WeeklyNotificationUserPreference? UserPreference { get; }
}