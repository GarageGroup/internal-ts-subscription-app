namespace GarageGroup.Internal.Timesheet;

internal interface IProjectJson
{
    string? Name { get; }

    string LookupValue { get; }

    string LookupEntity { get; }
}
