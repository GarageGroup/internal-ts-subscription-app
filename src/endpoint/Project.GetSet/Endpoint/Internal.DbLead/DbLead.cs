using GarageGroup.Infra;
using System;
using System.Text;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("lead", AliasName)]
public sealed partial record class DbLead : IDbEntity<DbLead>, IDbProject
{
    private const string All = "QueryAll";

    private const string AliasName = "l";

    ProjectType IDbProject.ProjectType
        =>
        ProjectType.Lead;

    string? IDbProject.ProjectName
    {
        get
        {
            if (string.IsNullOrEmpty(CompanyName))
            {
                return Subject;
            }

            var builder = new StringBuilder(Subject);
            if (string.IsNullOrEmpty(Subject) is false)
            {
                builder = builder.Append(' ');
            }

            return builder.Append('(').Append(CompanyName).Append(')').ToString();
        }
    }

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
