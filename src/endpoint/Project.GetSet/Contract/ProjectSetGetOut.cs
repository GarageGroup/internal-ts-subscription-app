using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static ProjectSetGetMetadata;

public readonly record struct ProjectSetGetOut
{
    [JsonBodyOut, SwaggerDescription(Out.ProjectsDescription)]
    public required FlatArray<ProjectItem> Projects { get; init; }
}