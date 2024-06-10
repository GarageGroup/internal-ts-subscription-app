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

    internal static DataverseEntityUpdateIn<NotificationSubscriptionJson> BuildDataverseUpdateInput(
        Guid botUserId, 
        Guid typeId, 
        NotificationSubscriptionJson subscription, 
        DataverseUpdateOperationType operationType = DataverseUpdateOperationType.Upsert) 
        => 
        new(
            entityPluralName: EntityPluralName, 
            entityKey: new DataverseAlternateKey(
                [
                    new (BotUserIdFieldName, botUserId.ToString()),
                    new (NotificationTypeIdFieldName, typeId.ToString()),
                ]),
            entityData: subscription)
        {
            OperationType = operationType
        };

    [JsonPropertyName(DisabledStatusFieldName)]
    public bool IsDisabled { get; init; }

    [JsonPropertyName(NotificationPreferencesFieldName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserPreferences { get; init; }
}