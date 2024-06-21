namespace GarageGroup.Internal.Timesheet;

internal static class TagSetGetMetadata
{
    public static class Func
    {
        public const string Tag = "Tag";

        public const string Route = "/getTags";

        public const string Summary
            =
            "Get tags";

        public const string Description
            =
            "Retrieves a list of tags associated with user's timesheets.";
    }

    public static class In
    {
        public const string ProjectIdDescription
            =
            "Unique identifier of the project in Dataverse.";

        public const string ProjectIdExample = "9dfb0e67-e565-4787-bab6-d92a2cf6bb70";
    }

    public static class Out
    {
        public const string TagsDescription
            =
            "Array of tags associated with user's timesheets.";

        public const string TagExample = "#Task8137";
    }
}