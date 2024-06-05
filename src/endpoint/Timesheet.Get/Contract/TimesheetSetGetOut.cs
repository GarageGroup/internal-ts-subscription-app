using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetSetGetOut
{
    [JsonBodyOut]
    public required FlatArray<TimesheetSetGetItem> Timesheets { get; init; }
}