using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static LastProjectSetGetMetadata;

public readonly record struct LastProjectSetGetOut
{
    [JsonBodyOut, SwaggerDescription(Out.ProjectsDescription)]
    public required FlatArray<ProjectItem> Projects { get; init; }
}