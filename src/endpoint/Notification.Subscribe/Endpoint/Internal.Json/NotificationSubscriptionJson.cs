using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class NotificationSubscriptionJson
{
    internal static DataverseEntityCreateIn<NotificationSubscriptionJson> BuildDataverseCreateInput(NotificationSubscriptionJson notificationSubscription) 
        =>
        new (
            entityPluralName: NotificationSubscriptionEntity.EntityPluralName,
            entityData: notificationSubscription);
    
    internal static DataverseEntityUpdateIn<NotificationSubscriptionJson> BuildDataverseUpdateInput(Guid subscriptionId, NotificationSubscriptionJson subscription) 
        => 
        new (
            entityPluralName: NotificationSubscriptionEntity.EntityPluralName, 
            entityKey: new DataversePrimaryKey(subscriptionId), 
            entityData: subscription);

    internal static string BuildBotUserLookupValue(Guid botUserId)
        => 
        $"/{TelegramBotUserJson.EntityPluralName}({botUserId:D})";

    internal static string BuildNotificationTypeLookupValue(Guid notificationTypeId)
        =>
        $"/{NotificationTypeJson.EntityPluralName}({notificationTypeId:D})";
    
    
    [JsonPropertyName($"{NotificationSubscriptionEntity.BotUserLookupName}@odata.bind")]
    public string? BotUserId { get; init; }
    
    [JsonPropertyName($"{NotificationSubscriptionEntity.NotificationTypeLookupName}@odata.bind")]
    public string? NotificationType { get; init; }
    
    [JsonPropertyName(NotificationSubscriptionEntity.DisabledStatusFieldName)]
    public bool? IsDisabled { get; init; }
    
    [JsonPropertyName(NotificationSubscriptionEntity.NotificationPreferencesFieldName)]
    public string? NotificationPreferences { get; init; }
}