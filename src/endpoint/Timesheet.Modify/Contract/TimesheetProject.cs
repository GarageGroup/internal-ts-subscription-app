using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TimesheetProject
{
    public TimesheetProject(Guid id, ProjectType type)
    {
        Id = id;
        Type = type;
    }

    public Guid Id { get; }

    public ProjectType Type { get; }
}