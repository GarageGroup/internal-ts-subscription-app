using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbOpportunity
{
    internal static readonly DbRawFilter IsActiveFilter
        =
        new($"{AliasName}.statecode = 0");
}