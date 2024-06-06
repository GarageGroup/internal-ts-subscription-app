using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TagGetSetOut
{
    [JsonBodyOut]
    public required FlatArray<string> Tags { get; init; }
}