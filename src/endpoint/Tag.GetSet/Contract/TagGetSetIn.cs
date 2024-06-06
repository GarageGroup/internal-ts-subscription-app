using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TagGetSetIn
{
    public TagGetSetIn(
        [ClaimIn] Guid systemUserId, 
        [JsonBodyIn] Guid projectId, 
        [JsonBodyIn] DateOnly minDate, 
        [JsonBodyIn] DateOnly maxDate)
    {
        SystemUserId = systemUserId;
        ProjectId = projectId;
        MinDate = minDate;
        MaxDate = maxDate;
    }

    public Guid SystemUserId { get; }

    public Guid ProjectId { get; }

    public DateOnly MinDate { get; }

    public DateOnly MaxDate { get; }
}