using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetSetGetMetadata;

public sealed record class TimesheetSetGetItem
{
    public TimesheetSetGetItem(
        Guid id,
        Guid projectId,
        ProjectType projectType,
        [AllowNull] string projectName,
        decimal duration,
        [AllowNull] string description,
        [AllowNull] StateCode? incidentStateCode,
        StateCode timesheetStateCode,
        DateOnly date)
    { 
        Id = id;
        ProjectId = projectId;
        ProjectType = projectType;
        ProjectName = projectName.OrEmpty();
        Duration = duration;
        Description = description.OrEmpty();
        IncidentStateCode = incidentStateCode;
        TimesheetStateCode = timesheetStateCode;
        Date = date;
    }

    [SwaggerDescription(Out.IdDescription)]
    [StringExample(Out.IdExample)]
    public Guid Id { get; }

    [SwaggerDescription(Out.ProjectIdDescription)]
    [StringExample(Out.ProjectIdExample)]
    public Guid ProjectId { get; }

    [SwaggerDescription(Out.ProjectTypeDescription)]
    [StringExample(Out.ProjectTypeExample)]
    public ProjectType ProjectType { get; }

    [SwaggerDescription(Out.ProjectNameDescription)]
    [StringExample(Out.ProjectNameExample)]
    public string ProjectName { get; }

    [SwaggerDescription(Out.DurationDescription)]
    [IntegerExample(Out.DurationExample)]
    public decimal Duration { get; }

    [SwaggerDescription(Out.DescriptionDescription)]
    [StringExample(Out.DescriptionExample)]
    public string Description { get; }

    [SwaggerDescription(Out.IncidentStateCodeDescription)]
    [StringExample(Out.IncidentStateCodeExample)]
    public StateCode? IncidentStateCode { get; }

    [SwaggerDescription(Out.TimesheetStateCodeDescription)]
    [StringExample(Out.TimesheetStateCodeExample)]
    public StateCode TimesheetStateCode { get; }

    [SwaggerDescription(Out.DateDescription)]
    public DateOnly Date { get; }
}
