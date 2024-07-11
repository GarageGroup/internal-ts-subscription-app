using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbOpportunity
{
    [DbSelect(All, AliasName, $"{AliasName}.opportunityid")]
    public Guid ProjectId { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.name")]
    public string? ProjectName { get; init; }
}