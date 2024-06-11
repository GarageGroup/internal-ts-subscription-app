using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TimesheetProject
{
    public TimesheetProject(Guid id, ProjectType type)
    {
        Id = id;
        Type = type;
    }

    public Guid Id { get; }

    [StringExample(nameof(ProjectType.Project))]
    public ProjectType Type { get; }
}