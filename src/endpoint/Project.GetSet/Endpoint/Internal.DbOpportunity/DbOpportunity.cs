using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("opportunity", AliasName)]
internal sealed partial record class DbOpportunity : IDbEntity<DbOpportunity>, IDbProject
{
    private const string All = "QueryAll";

    private const string AliasName = "o";

    ProjectType IDbProject.ProjectType
        =>
        ProjectType.Opportunity;

    string? IDbProject.ProjectComment
        =>
        null;

    DateTime? IDbProject.UserLastTimesheetDate
        =>
        null;

    DateTime? IDbProject.LastTimesheetDate
        =>
        null;
}
