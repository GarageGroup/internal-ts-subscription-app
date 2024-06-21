using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetModifyMetadata;

public sealed record class TimesheetProject
{
    public TimesheetProject(Guid id, ProjectType type)
    {
        Id = id;
        Type = type;
    }

    [SwaggerDescription(In.ProjectIdDescription)]
    [StringExample(In.ProjectIdExample)]
    public Guid Id { get; }

    [SwaggerDescription(In.ProjectTypeDescription)]
    [StringExample(In.ProjectTypeExample)]
    public ProjectType Type { get; }
}