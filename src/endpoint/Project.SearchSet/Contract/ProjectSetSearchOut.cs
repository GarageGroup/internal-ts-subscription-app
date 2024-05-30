using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct ProjectSetSearchOut
{
    [JsonBodyOut]
    public required FlatArray<ProjectItem> Projects { get; init; }
}