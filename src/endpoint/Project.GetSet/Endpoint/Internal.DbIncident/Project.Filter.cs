using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbIncident
{
    internal static readonly DbRawFilter IsActiveFilter
        =
        new($"{AliasName}.statecode = 0");
}