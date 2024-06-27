using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class SubscriptionJson
{
    private const string EntityPluralName = "gg_bot_user_subscriptions";

    private const string BotSystemUserFieldName = "gg_bot_user_id/_gg_systemuser_id_value";

    private const string BotIdFieldName = "gg_bot_user_id/gg_bot_id";

    private const string IsDisabledFieldName = "gg_is_disabled";

    private const string NotificationTypeLookup = "gg_notification_type_id";

    private const string NotificationPreferencesFieldName = "gg_notification_preferences";

    private static readonly FlatArray<string> SelectedFields
        =
        new(NotificationPreferencesFieldName);

    private static readonly FlatArray<DataverseExpandedField> ExpandFields
        = 
        [
            new(NotificationTypeLookup, [NotificationTypeJson.KeyFieldName])
        ];

    public static DataverseEntitySetGetIn BuildGetInput(Guid systemUserId, long botId)
        =>
        new(
            entityPluralName: EntityPluralName,
            selectFields: SelectedFields,
            expandFields: ExpandFields,
            filter: new DataverseLogicalFilter(DataverseLogicalOperator.And)
            {
                Filters =
                [
                    new DataverseComparisonFilter(BotSystemUserFieldName, DataverseComparisonOperator.Equal, systemUserId),
                    new DataverseComparisonFilter(BotIdFieldName, DataverseComparisonOperator.Equal, botId.ToString()),
                    new DataverseComparisonFilter(IsDisabledFieldName, DataverseComparisonOperator.Inequal, true)
                ]
            });

    [JsonPropertyName(NotificationTypeLookup)]
    public NotificationTypeJson? NotificationType { get; init; }

    [JsonPropertyName(NotificationPreferencesFieldName)]
    public string? UserPreference { get; init; }
}