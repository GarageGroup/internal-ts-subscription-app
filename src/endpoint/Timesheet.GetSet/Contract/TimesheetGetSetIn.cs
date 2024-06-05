using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetGetSetIn
{
    public TimesheetGetSetIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] DateOnly date)
    {
        SystemUserId = systemUserId;
        Date = date;
    }

    public Guid SystemUserId { get; }

    public DateOnly Date { get; }
}