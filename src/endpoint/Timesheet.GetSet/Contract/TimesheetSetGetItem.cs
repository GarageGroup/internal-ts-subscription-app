using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TimesheetSetGetItem
{
    public TimesheetSetGetItem(
        decimal duration,
        Guid projectId,
        ProjectType projectType,
        [AllowNull] string projectName,
        [AllowNull] string description,
        Guid id,
        [AllowNull] StateCode? incidentStateCode,
        StateCode timesheetStateCode,
        DateOnly date)
    { 
        ProjectId = projectId;
        ProjectType = projectType;
        ProjectName = projectName.OrEmpty();
        Duration = duration;
        Description = description.OrEmpty();
        Id = id;
        IncidentStateCode = incidentStateCode;
        TimesheetStateCode = timesheetStateCode;
        Date = date;
    }

    public Guid ProjectId { get; }

    [StringExample(nameof(ProjectType.Project))]
    public ProjectType ProjectType { get; }

    public string ProjectName { get; }

    public decimal Duration { get; }

    public string Description { get; }

    public Guid Id { get; }

    public StateCode? IncidentStateCode { get; }

    public StateCode TimesheetStateCode { get; }

    public DateOnly Date { get; }
}
