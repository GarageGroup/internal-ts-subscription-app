using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static ProjectSetSearchMetadata;

public readonly record struct ProjectSetSearchOut
{
    [JsonBodyOut, SwaggerDescription(Out.ProjectsDescription)]
    public required FlatArray<ProjectItem> Projects { get; init; }
}