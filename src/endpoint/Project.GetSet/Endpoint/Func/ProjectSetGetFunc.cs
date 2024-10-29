using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectSetGetFunc(ISqlQueryEntitySetSupplier sqlApi, ITodayProvider todayProvider) : IProjectSetGetFunc
{
    private const int LastDaysPeriod = 30;

    private static readonly DbSelectQuery DbIncidentQueryAll
        =
        DbIncident.QueryAll with
        {
            Filter = DbIncident.IsActiveFilter
        };

    private static readonly DbSelectQuery DbLeadQueryAll
        =
        DbLead.QueryAll with
        {
            Filter = DbLead.IsActiveFilter
        };

    private static readonly DbSelectQuery DbOpportunityQueryAll
        =
        DbOpportunity.QueryAll with
        {
            Filter = DbOpportunity.IsActiveFilter
        };

    private static ProjectItem MapProject(IDbProject dbProject)
        =>
        new(
            id: dbProject.ProjectId,
            name: dbProject.ProjectName,
            type: dbProject.ProjectType)
        {
            Comment = dbProject.ProjectComment.OrNullIfWhiteSpace()
        };

    private static string? GetProjectName(IDbProject projectItem)
        =>
        projectItem.ProjectName;

    private static DateTime? GetUserLastTimesheetDate(IDbProject projectItem)
        =>
        projectItem.UserLastTimesheetDate;

    private static DateTime? GetLastTimesheetDate(IDbProject projectItem)
        =>
        projectItem.LastTimesheetDate;
}