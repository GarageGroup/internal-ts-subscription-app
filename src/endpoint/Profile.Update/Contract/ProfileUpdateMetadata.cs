namespace GarageGroup.Internal.Timesheet;

internal static class ProfileUpdateMetadata
{
    public static class Func
    {
        public const string Tag = "Profile";

        public const string Route = "/updateProfile";

        public const string Summary
            =
            "Update profile";

        public const string Description
            =
            "Updates the profile information of the authorized user.";
    }

    public static class In
    {
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