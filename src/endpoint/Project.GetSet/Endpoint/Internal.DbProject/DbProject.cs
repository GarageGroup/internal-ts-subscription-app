using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_project", AliasName)]
public sealed partial record class DbProject : IDbEntity<DbProject>, IDbProject
{
    private const string All = "QueryAll";

    private const string AliasName = "p";

    private const string UserLastTimesheetDateAliasName = "user_last_timesheet_date";

    private const string LastTimesheetDateAliasName = "last_timesheet_date";

    ProjectType IDbProject.ProjectType => ProjectType.Project;
}