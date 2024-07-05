namespace GarageGroup.Internal.Timesheet;

internal static class UserSignInMetadata
{
    public static class Func
    {
        public const string Tag = "Authorization";

        public const string Route = "/signIn";

        public const string Summary
            =
            "Sign in";

        public const string Description
            =
            "Log in the user.";
    }

    public static class In
    {
        public const string ChatIdDescription
            =
            "Unique identifier of the telegram chat.";

        public const int ChatIdExample
            =
            185921581;
    }

    public static class FailureCode
    {
        public const string SystemUserNotFoundMessage
            =
            "System user was not found";

        public const string InvalidTelegramDataMessage
            =
            "Telegram data is invalid";
    }
}