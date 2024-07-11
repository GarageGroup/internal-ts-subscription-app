using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_project", AliasName)]
public sealed partial record class DbProject : IDbEntity<DbProject>
{
    private const string All = "QueryAll";

    private const string AliasName = "p";
}