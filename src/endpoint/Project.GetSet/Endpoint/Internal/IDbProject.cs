using System;

namespace GarageGroup.Internal.Timesheet;

internal interface IDbProject
{
    ProjectType ProjectType { get; }

    Guid ProjectId { get; }

    string? ProjectName { get; }

    string? ProjectComment { get; }

    DateTime? UserLastTimesheetDate { get; }

    DateTime? LastTimesheetDate { get; }
}