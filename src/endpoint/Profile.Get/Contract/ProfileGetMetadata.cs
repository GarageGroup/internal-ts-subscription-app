namespace GarageGroup.Internal.Timesheet;

internal static class ProfileGetMetadata
{
    public static class Func
    {
        public const string Tag = "Profile";

        public const string Route = "/getProfile";

        public const string Summary
            =
            "Get profile";

        public const string Description
            =
            "Retrieves profile information of the authorized user.";
    }

    public static class Out
    {
        public const string UserNameDescription
            =
            "User's name.";

        public const string UserNameExample
            =
            "John Doe";

        public const string LanguageCodeDescription
            =
            "ISO code representing the user's language preference.";

        public const string LanguageCodeExample
            =
            "en";
    }

    public static class FailureCode
    {
        public const string NotFoundMessage
            =
            "Profile was not found";
    }
}