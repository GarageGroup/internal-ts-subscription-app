namespace GarageGroup.Internal.Timesheet;

internal static class UserSignOutMetadata
{
    public static class Func
    {
        public const string Tag = "Authorization";

        public const string Route = "/signOut";

        public const string Summary
            =
            "Sign out";

        public const string Description
            =
            "Log out the user.";
    }
}