namespace GarageGroup.Internal.Timesheet;

internal static class NotificationSubscriptionEntity
{
    public const string EntityPluralName = "gg_bot_user_subscriptions";

    public const string IdFieldName = "gg_bot_user_subscriptionid";

    public const string BotUserIdFieldName = "_gg_bot_user_id_value";

    public const string BotUserLookupName = "gg_bot_user_id";

    public const string NotificationTypeIdFieldName = "_gg_notification_type_id_value";
    
    public const string NotificationTypeLookupName = "gg_notification_type_id";
    
    public const string NotificationPreferencesFieldName = "gg_notification_preferences";

    public const string DisabledStatusFieldName = "gg_is_disabled";
}