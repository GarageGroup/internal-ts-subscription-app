using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct LastProjectSetGetIn
{
    public LastProjectSetGetIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn, IntegerExample(50)] int? top)
    {
        SystemUserId = systemUserId;
        Top = top;
    }

    public Guid SystemUserId { get; }

    public int? Top { get; }
}