using System;
using System.Text.Json;
using GarageGroup.Infra;
using GarageGroup.Infra.Endpoint;

namespace GarageGroup.Internal.Timesheet;

[MediaTypeMetadata(TypeName = "DailyNotificationMetadata")]
public sealed record class DailyNotificationSubscriptionData : BaseSubscriptionData
{
    internal static Result<BaseSubscriptionData, Failure<Unit>> DeserializeOrFailure(
        JsonDocument jsonDocument, JsonSerializerOptions serializerOptions)
    {
        var preferenceResult = jsonDocument.DeserializeOrFailure<DailyNotificationUserPreference?>("userPreference", serializerOptions);
        return preferenceResult.MapSuccess(CreateDailyNotificationSubscriptionData);

        static BaseSubscriptionData CreateDailyNotificationSubscriptionData(DailyNotificationUserPreference? userPreference)
            =>
            new DailyNotificationSubscriptionData(userPreference);
    }

    public DailyNotificationSubscriptionData(DailyNotificationUserPreference? userPreference)
        => 
        UserPreference = userPreference;

    [StringExample(nameof(NotificationType.DailyNotification))]
    public override NotificationType NotificationType { get; } = NotificationType.DailyNotification;

    public override DailyNotificationUserPreference? UserPreference { get; }
}