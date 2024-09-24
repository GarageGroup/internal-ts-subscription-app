using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_timesheetactivity", AliasName)]
[DbJoin(DbJoinType.Left, "gg_project", ProjectAlias, JoinedProjectSql)]
public sealed partial record class DbLastProject : IDbEntity<DbLastProject>
{
    private const string All = "QueryAll";

    private const string AliasName = "t";

    private const string ProjectAlias = "p";

    private const string ProjectTypeCodeFieldName = $"{AliasName}.regardingobjecttypecode";

    private const string JoinedProjectSql
        =
        $"{AliasName}.regardingobjecttypecode = 10912 AND {AliasName}.regardingobjectid = {ProjectAlias}.gg_projectid";

    private const string SelectLastSubjectSql
        =
        "(SELECT TOP 1 sub.subject FROM gg_timesheetactivity sub" +
        $" WHERE sub.regardingobjectid = {AliasName}.regardingobjectid ORDER BY sub.createdon DESC)";
}