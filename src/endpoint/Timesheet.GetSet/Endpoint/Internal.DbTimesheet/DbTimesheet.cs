using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_timesheetactivity", AliasName)]
[DbJoin(DbJoinType.Left, "incident", IncidentAlias, $"{AliasName}.regardingobjectid = {IncidentAlias}.incidentid")]
internal sealed partial record class DbTimesheet : IDbEntity<DbTimesheet>
{
    private const string All = "QueryAll";

    private const string AliasName = "t";

    private const string IncidentAlias = "i";

    private const string DateFormat = "yyyy-MM-dd";

    private const string SelectLastSubjectSql = "(SELECT TOP 1 sub.subject " +
                                                "FROM gg_timesheetactivity sub " +
                                                "WHERE sub.regardingobjectid = t.regardingobjectid " +
                                                "ORDER BY sub.createdon DESC)";
}