using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using static ProjectSetGetMetadata;

public sealed record class ProjectItem
{
    public ProjectItem(Guid id, [AllowNull] string name, ProjectType type)
    {
        Id = id;
        Name = name.OrEmpty();
        Type = type;
    }

    [SwaggerDescription(Out.IdDescription)]
    [StringExample(Out.IdExample)]
    public Guid Id { get; }

    [SwaggerDescription(Out.NameDescription)]
    [StringExample(Out.NameExample)]
    public string Name { get; }

    [SwaggerDescription(Out.TypeDescription)]
    [StringExample(Out.TypeExample)]
    public ProjectType Type { get; }
}