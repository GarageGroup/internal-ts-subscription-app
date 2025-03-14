using System.Text.Json;

namespace GarageGroup.Internal.Timesheet;

internal static class SubscriptionSetGetMetadata
{
    public static readonly JsonNamingPolicy NamingPolicy
        =
        JsonNamingPolicy.CamelCase;

    public static class Func
    {
        public const string Tag = "Notification";

        public const string Route = "/";

        public const string Summary
            =
            "Get notification subscriptions for an user";

        public const string Description
            =
            "Retrieves a list of notification subscriptions for an authenticated user";
    }

    public static class In
    {
        public const string NotificationTypeDesciption
            =
            "Type of notifications for the bot user";

        public const string NotificationTimeDescription
            =
            "Preferred notification time in the Moscow timezone (UTC+3)";

        public const string NotificationTimeExample = "18:00";

        public const string DailyNotificationUserPreferenceDesciption
            =
            "User's daily notification preferences";

        public const string DailyNotificationWorkedHoursDescription
            =
            "Number of hours worked in a day";

        public const int DailyNotificationWorkedHoursExample = 8;

        public const string WeeklyNotificationUserPreferenceDesciption
            =
            "User's weekly notification preferences";

        public const string WeeklyNotificationWorkedHoursDescription
            =
            "Number of hours worked in a week";

        public const int WeeklyNotificationWorkedHoursExample = 40;

        public const string WeeklyNotificationWeekdayDescription
            =
            "Day or days when notifications are sent";

        public const string WeeklyNotificationWeekdayExample = nameof(Weekday.Friday);
    }

    public static class Out
    {
        public const string SubscriptionsDescription
            =
            "Notification subscriptions for an user";

        public const string NotificationTypeDescription
            =
            "Specifies the type of notification that the user is subscribed to";

        public const string UserPreferenceDescription
            =
            "Represents the user's preferences or settings related to notifications of the specified type";
    }

    public static class FailureCode
    {
        public const string BotUserNotFoundMessage
            =
            "Bot user was not found";
    }
}