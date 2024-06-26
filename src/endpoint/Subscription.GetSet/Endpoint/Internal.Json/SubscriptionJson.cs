using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet.Inner.Json;

internal sealed record class SubscriptionJson
{
    private const string EntityPluralName = "gg_bot_user_subscriptions";

    private const string IdFieldName = "gg_bot_user_subscriptionid";

    private const string BotUserFieldName = "_gg_bot_user_id_value";

    private const string NotificationTypeLookup = "gg_notification_type_id";

    private const string NotificationPreferencesFieldName = "gg_notification_preferences";

    private const string IsDisabledFieldName = "gg_is_disabled";

    private static readonly FlatArray<string> SelectedFields 
        = 
        [IdFieldName, NotificationPreferencesFieldName, IsDisabledFieldName];
    
    private static readonly FlatArray<DataverseExpandedField> ExpandFields 
        = 
        [new DataverseExpandedField(NotificationTypeLookup, [NotificationTypeJson.KeyFieldName])];
    
    public static DataverseEntitySetGetIn BuildGetInput(Guid botUserId) 
        => 
        new (
            entityPluralName: EntityPluralName,
            selectFields: SelectedFields,
            expandFields: ExpandFields,
            filter: $"{BotUserFieldName} eq '{botUserId}' and {IsDisabledFieldName} eq false");
    
    [JsonPropertyName(IdFieldName)]
    public Guid Id { get; init; }
    
    [JsonPropertyName(NotificationTypeLookup)]
    public required NotificationTypeJson NotificationType { get; init; }
    
    [JsonPropertyName(NotificationPreferencesFieldName)]
    public string? UserPreference { get; init; }
}