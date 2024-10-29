using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectSetGetFunc
{
    public ValueTask<Result<ProjectSetGetOut, Failure<Unit>>> InvokeAsync(
        ProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeParallelValue(
            GetIncidentsAsync,
            GetProjectsAsync,
            GetOpportunitiesAsync,
            GetLeadsAsync)
        .MapSuccess(
            static result => MapSuccess(result.Item1, result.Item2, result.Item3, result.Item4));

    private ValueTask<Result<FlatArray<DbProject>, Failure<Unit>>> GetProjectsAsync(
        ProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => DbProject.QueryAll with
            {
                Filter = DbProject.IsActiveFilter,
                AppliedTables = DbProject.BuildTimesheetDateDbAppliedTables(@in.SystemUserId, todayProvider.Today.AddDays(-LastDaysPeriod))
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbProject>);

    private ValueTask<Result<FlatArray<DbOpportunity>, Failure<Unit>>> GetOpportunitiesAsync(
        ProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            DbOpportunityQueryAll, cancellationToken)
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbOpportunity>);

    private ValueTask<Result<FlatArray<DbLead>, Failure<Unit>>> GetLeadsAsync(
        ProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            DbLeadQueryAll, cancellationToken)
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbLead>);

    private ValueTask<Result<FlatArray<DbIncident>, Failure<Unit>>> GetIncidentsAsync(
        ProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            DbIncidentQueryAll, cancellationToken)
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbIncident>);

    private static ProjectSetGetOut MapSuccess(
        FlatArray<DbIncident> incidents,
        FlatArray<DbProject> projects, 
        FlatArray<DbOpportunity> opportunities, 
        FlatArray<DbLead> leads)
    {
        return new()
        {
            Projects =
                Pipeline.Pipe(
                    projects.CastArray<IDbProject>()
                    .Concat(opportunities.CastArray<IDbProject>())
                    .Concat(leads.CastArray<IDbProject>())
                    .Concat(incidents.CastArray<IDbProject>()))
                .AsEnumerable()
                .OrderByDescending(GetUserLastTimesheetDate)
                .ThenByDescending(GetLastTimesheetDate)
                .ThenBy(GetName)
                .Select(MapProject)
                .ToFlatArray()
        };

        static ProjectItem MapProject(IDbProject dbProject)
            =>
            new(
                id: dbProject.ProjectId,
                name: dbProject.ProjectName,
                type: dbProject.ProjectType)
            {
                Comment = dbProject.ProjectComment.OrNullIfWhiteSpace()
            };

        static string? GetName(IDbProject projectItem)
            =>
            projectItem.ProjectName;

        static DateTime? GetUserLastTimesheetDate(IDbProject projectItem)
            =>
            projectItem.UserLastTimesheetDate;

        static DateTime? GetLastTimesheetDate(IDbProject projectItem)
            =>
            projectItem.LastTimesheetDate;
    }
}