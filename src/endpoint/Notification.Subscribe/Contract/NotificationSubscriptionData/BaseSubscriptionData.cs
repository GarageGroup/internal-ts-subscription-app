using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

[JsonConverter(typeof(SubscriptionDataJsonConverter))]
public abstract record class BaseSubscriptionData
{
    public abstract NotificationType NotificationType { get; }

    public abstract INotificationUserPreference? UserPreference { get; }
}