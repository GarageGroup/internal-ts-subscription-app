using System;
using System.Text.Json;
using GarageGroup.Infra;
using GarageGroup.Infra.Endpoint;

namespace GarageGroup.Internal.Timesheet;

using static NotificationSubscribeMetadata;

[MediaTypeMetadata(TypeName = "WeeklyNotificationMetadata")]
public sealed record class WeeklyNotificationSubscriptionData : BaseSubscriptionData
{
    internal static Result<BaseSubscriptionData, Failure<Unit>> DeserializeOrFailure(
        JsonDocument jsonDocument, JsonSerializerOptions serializerOptions)
    {
        var preferenceResult = jsonDocument.DeserializeOrFailure<WeeklyNotificationUserPreference?>("userPreference", serializerOptions);
        return preferenceResult.MapSuccess(CreateDailyNotificationSubscriptionData);

        static BaseSubscriptionData CreateDailyNotificationSubscriptionData(WeeklyNotificationUserPreference? userPreference)
            =>
            new WeeklyNotificationSubscriptionData(userPreference);
    }

    public WeeklyNotificationSubscriptionData(WeeklyNotificationUserPreference? userPreference)
        => 
        UserPreference = userPreference;

    [SwaggerDescription(In.NotificationTypeDesciption)]
    [StringExample(nameof(NotificationType.WeeklyNotification))]
    public override NotificationType NotificationType { get; } = NotificationType.WeeklyNotification;

    [SwaggerDescription(In.WeeklyNotificationUserPreferenceDesciption)]
    public override WeeklyNotificationUserPreference? UserPreference { get; }
}