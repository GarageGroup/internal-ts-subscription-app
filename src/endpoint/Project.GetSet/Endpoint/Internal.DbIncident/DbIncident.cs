using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("incident", AliasName)]
public sealed partial record class DbIncident : IDbEntity<DbIncident>
{
    private const string All = "QueryAll";

    private const string AliasName = "i";
}