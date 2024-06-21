using System;
using System.Text.Json;
using GarageGroup.Infra;
using GarageGroup.Infra.Endpoint;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

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

    [SwaggerDescription(In.NotificationTypeDesciption)]
    [StringExample(nameof(NotificationType.DailyNotification))]
    public override NotificationType NotificationType { get; } = NotificationType.DailyNotification;

    [SwaggerDescription(In.DailyNotificationUserPreferenceDesciption)]
    public override DailyNotificationUserPreference? UserPreference { get; }
}