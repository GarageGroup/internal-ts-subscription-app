using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetSetGetMetadata;

public readonly record struct TimesheetSetGetOut
{
    [JsonBodyOut, SwaggerDescription(Out.TimesheetsDescription)]
    public required FlatArray<TimesheetSetGetItem> Timesheets { get; init; }
}