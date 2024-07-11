using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("opportunity", AliasName)]
public sealed partial record class DbOpportunity : IDbEntity<DbOpportunity>
{
    private const string All = "QueryAll";

    private const string AliasName = "o";
}