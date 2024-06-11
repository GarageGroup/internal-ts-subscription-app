using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetSetGetIn
{
    public TimesheetSetGetIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] DateOnly dateFrom,
        [JsonBodyIn] DateOnly dateTo)
    {
        SystemUserId = systemUserId;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    public Guid SystemUserId { get; }

    public DateOnly DateFrom { get; }

    public DateOnly DateTo { get; }
}