using GarageGroup.Infra;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TimesheetCreateIn
{
    public TimesheetCreateIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] DateOnly date,
        [JsonBodyIn] TimesheetProject project,
        [JsonBodyIn] decimal duration,
        [JsonBodyIn] [AllowNull] string description)
    {
        SystemUserId = systemUserId;
        Date = date;
        Description = description.OrNullIfEmpty();
        Duration = duration;
        Project = project;
    }

    public Guid SystemUserId { get; }

    public DateOnly Date { get; }

    public TimesheetProject Project { get; }

    public decimal Duration { get; }

    public string? Description { get; }
}