using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProject
{
    internal static readonly DbRawFilter IsActiveFilter
        =
        new($"{AliasName}.statecode = 0");
}