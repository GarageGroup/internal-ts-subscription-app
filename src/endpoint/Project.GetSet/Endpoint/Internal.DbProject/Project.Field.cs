using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProject
{
    [DbSelect(All, AliasName, $"{AliasName}.gg_projectid")]
    public Guid ProjectId { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.gg_name")]
    public string? ProjectName { get; init; }

    [DbSelect(All, AliasName, $"{AliasName}.gg_comment")]
    public string? ProjectComment { get; init; }

    [DbSelect(All, AliasName, $"{UserLastTimesheetDateAliasName}.LastDay")]
    public DateTime? UserLastTimesheetDate { get; init; }

    [DbSelect(All, AliasName, $"{LastTimesheetDateAliasName}.LastDay")]
    public DateTime? LastTimesheetDate { get; init; }
}