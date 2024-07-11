using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbIncident
{
    [DbSelect(All, AliasName, $"{AliasName}.incidentid")]
    public Guid ProjectId { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.title")]
    public string? ProjectName { get; init; }
}