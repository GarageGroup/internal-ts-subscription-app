using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbLead
{
    [DbSelect(All, AliasName, $"{AliasName}.leadid")]
    public Guid ProjectId { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.companyname")]
    public string? CompanyName { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.subject")]
    public string? Subject { get; init; }
}