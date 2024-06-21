namespace GarageGroup.Internal.Timesheet;

internal static class ProjectSetSearchMetadata
{
    public static class Func
    {
        public const string Tag = "Project";

        public const string Route = "/searchProjects";

        public const string Summary
            =
            "Search for projects, leads, opportunities and incidents";

        public const string Description
            =
            "Retrieves a list of projects, leads, opportunities, and incidents matching a specified search criteria.";
    }

    public static class In
    {
        public const string SearchTextDescription
            =
            "Keyword used to search across projects, leads, opportunities and incidents.";

        public const string SearchTextExample = "Internal Timesheet";

        public const string TopDescription
            =
            "The maximum number of search results to return.";

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

        public const string IdExample = "90ee8a59-0c50-4164-9252-9a9f38f55674";

        public const string NameDescription
            =
            "Name of the project, lead, opportunity or incident.";

        public const string NameExample = "Internal Timesheet";

        public const string TypeDescription
            =
            "Type of the project";

        public const string TypeExample = nameof(ProjectType.Project);
    }

    public static class FailureCode
    {
        public const string ForbiddenMessage
            =
            "This method is forbidden for your account";
    }
}