using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static TagSetGetMetadata;

public readonly record struct TagSetGetOut
{
    [JsonBodyOut]
    [SwaggerDescription(Out.TagsDescription)]
    [StringExample(Out.TagExample)]
    public required FlatArray<string> Tags { get; init; }
}