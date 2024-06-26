namespace GarageGroup.Internal.Timesheet;

internal static class SubscriptionSetGetMetadata
{
    public static class Func
    {
        public const string Tag = "Notification";

        public const string Route = "/getSubscriptions";

        public const string Summary
            =
            "Get notification subscriptions for an user";

        public const string Description
            =
            "Retrieves a list of notification subscriptions for an authenticated user.";
    }

    public static class Out
    {
        public const string IdDescription = "Represents the unique identifier of the notification subscription.";

        public const string IdExample = "8059bf1e-e3f2-426c-802e-7213c83fcf31";
        
        public const string NotificationTypeDescription = "Specifies the type of notification that the user is subscribed to.";

        public const string NotificationTypeExample = "DailyNotification";
        
        public const string UserPreferenceDescription = "Represents the user's preferences or settings related to notifications of the specified type.";
    }
}