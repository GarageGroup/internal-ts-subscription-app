using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("incident", AliasName)]
internal sealed partial record class DbIncident : IDbEntity<DbIncident>, IDbProject
{
    private const string All = "QueryAll";

    private const string AliasName = "i";

    ProjectType IDbProject.ProjectType
        =>
        ProjectType.Incident;

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
