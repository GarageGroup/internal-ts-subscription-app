using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class NotificationSubscriptionJson
{
    private const string EntityPluralName = "gg_bot_user_subscriptions";

    private const string BotUserIdFieldName = "_gg_bot_user_id_value";

    private const string NotificationTypeIdFieldName = "_gg_notification_type_id_value";
    
    private const string NotificationPreferencesFieldName = "gg_notification_preferences";

    private const string DisabledStatusFieldName = "gg_is_disabled";
    
    internal static DataverseEntityUpdateIn<NotificationSubscriptionJson> BuildDataverseUpsertInput(Guid botUserId, Guid typeId, NotificationSubscriptionJson subscription) 
        => 
        new (
            entityPluralName: EntityPluralName, 
            entityKey: new DataverseAlternateKey(
            [
                new (BotUserIdFieldName, botUserId.ToString()),
                new (NotificationTypeIdFieldName, typeId.ToString()),
            ]), 
            entityData: subscription)
        {
            OperationType = DataverseUpdateOperationType.Upsert
        };
    
    [JsonPropertyName(DisabledStatusFieldName)]
    public bool? IsDisabled { get; init; }
    
    [JsonPropertyName(NotificationPreferencesFieldName)]
    public string? NotificationPreferences { get; init; }
}