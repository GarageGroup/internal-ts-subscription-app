using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TimesheetUpdateIn
{
    public TimesheetUpdateIn(
        [JsonBodyIn] Guid timesheetId,
        [JsonBodyIn] DateOnly? date,
        [JsonBodyIn] TimesheetProject? project,
        [JsonBodyIn] decimal? duration,
        [JsonBodyIn] string? description)
    {
        TimesheetId = timesheetId;
        Date = date;
        Project = project;
        Duration = duration;
        Description = description;
    }

    public Guid TimesheetId { get; }

    public DateOnly? Date { get; }

    public TimesheetProject? Project { get; }

    public decimal? Duration { get; }

    public string? Description { get; }
}