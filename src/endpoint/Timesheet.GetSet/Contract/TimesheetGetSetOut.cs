using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetGetSetOut
{
    [JsonBodyOut]
    public required FlatArray<TimesheetGetSetItem> Timesheets { get; init; }
}