using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetSetGetIn
{
    public TimesheetSetGetIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] DateOnly date)
    {
        SystemUserId = systemUserId;
        Date = date;
    }

    public Guid SystemUserId { get; }

    public DateOnly Date { get; }
}