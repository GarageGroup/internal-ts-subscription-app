namespace GarageGroup.Internal.Timesheet;

internal static class LastProjectSetGetMetadata
{
    public static class Func
    {
        public const string Tag = "Project";

        public const string Route = "/getLastProjects";

        public const string Summary
            =
            "Get last user projects, leads, opportunities and incidents";

        public const string Description
            =
            "Retrieves the most recent projects, leads, opportunities, and incidents associated with the user.";
    }

    public static class In
    {
        public const string TopDescription
            =
            "The maximum number of results to return.";

        public const int TopExample = 50;
    }

    public static class Out
    {
        public const string ProjectsDescription
            =
            "Array of project items.";

        public const string IdDescription
            =
            "Unique identifier in Dataverse.";

        public const string IdExample = "839bb7ea-6e1f-4ba0-a5fb-a63769a20e65";

        public const string NameDescription
            =
            "Name of the project, lead, opportunity or incident.";

        public const string NameExample = "Internal Timesheet";

        public const string TypeDescription
            =
            "Type of the project";

        public const string TypeExample = nameof(ProjectType.Project);
    }
}