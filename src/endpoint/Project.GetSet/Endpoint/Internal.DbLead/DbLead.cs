using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("lead", AliasName)]
public sealed partial record class DbLead : IDbEntity<DbLead>
{
    private const string All = "QueryAll";

    private const string AliasName = "l";
}