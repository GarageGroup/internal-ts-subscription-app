using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_timesheetactivity", AliasName)]
public sealed partial record class DbLastProject : IDbEntity<DbLastProject>
{
    private const string All = "QueryAll";

    private const string AliasName = "t";

    private const string ProjectTypeCodeFieldName = $"{AliasName}.regardingobjecttypecode";

    private const string SelectLastSubjectSql = "(SELECT TOP 1 sub.subject " +
                                                "FROM gg_timesheetactivity sub " +
                                                "WHERE sub.regardingobjectid = t.regardingobjectid " +
                                                "ORDER BY sub.createdon DESC)";
}