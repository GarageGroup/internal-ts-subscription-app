using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TagSetGetOut
{
    [JsonBodyOut]
    public required FlatArray<string> Tags { get; init; }
}