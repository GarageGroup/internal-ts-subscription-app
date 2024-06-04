using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct LastProjectSetGetOut
{
    [JsonBodyOut]
    public required FlatArray<ProjectItem> Projects { get; init; }
}