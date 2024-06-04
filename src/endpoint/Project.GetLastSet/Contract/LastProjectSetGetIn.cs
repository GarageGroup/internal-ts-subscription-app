using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct LastProjectSetGetIn
{
    public LastProjectSetGetIn(
        [ClaimIn] Guid systemUserId, 
        [JsonBodyIn] int top, 
        [JsonBodyIn] DateOnly minDate)
    {
        SystemUserId = systemUserId;
        Top = top;
        MinDate = minDate;
    }

    public Guid SystemUserId { get; }

    public int Top { get; }

    public DateOnly MinDate { get; }
}