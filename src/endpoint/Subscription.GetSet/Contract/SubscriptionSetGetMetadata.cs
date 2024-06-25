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
}