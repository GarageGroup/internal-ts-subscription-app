using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectSetGetFunc
{
    public ValueTask<Result<ProjectSetGetOut, Failure<Unit>>> InvokeAsync(
        Unit input, CancellationToken cancellationToken)
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
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            DbProjectQueryAll, cancellationToken)
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbProject>);

    private ValueTask<Result<FlatArray<DbOpportunity>, Failure<Unit>>> GetOpportunitiesAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            DbOpportunityQueryAll, cancellationToken)
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbOpportunity>);

    private ValueTask<Result<FlatArray<DbLead>, Failure<Unit>>> GetLeadsAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            DbLeadQueryAll, cancellationToken)
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbLead>);

    private ValueTask<Result<FlatArray<DbIncident>, Failure<Unit>>> GetIncidentsAsync(
        Unit input, CancellationToken cancellationToken)
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
                    projects.AsEnumerable().Select(MapProject))
                .Concat(
                    opportunities.AsEnumerable().Select(MapOpportunity))
                .Concat(
                    leads.AsEnumerable().Select(MapLead))
                .Concat(
                    incidents.AsEnumerable().Select(MapIncident))
                .OrderBy(
                    GetName)
                .ToFlatArray()
        };

        static ProjectItem MapProject(DbProject dbProject)
            =>
            new(
                id: dbProject.ProjectId,
                name: dbProject.ProjectName,
                type: ProjectType.Project);

        static ProjectItem MapOpportunity(DbOpportunity dbOpportunity)
            =>
            new(
                id: dbOpportunity.ProjectId,
                name: dbOpportunity.ProjectName,
                type: ProjectType.Opportunity);

        static ProjectItem MapIncident(DbIncident dbIncident)
            =>
            new(
                id: dbIncident.ProjectId,
                name: dbIncident.ProjectName,
                type: ProjectType.Incident);

        static ProjectItem MapLead(DbLead dbLead)
            =>
            new(
                id: dbLead.ProjectId,
                name: BuildLeadName(dbLead).OrEmpty(),
                type: ProjectType.Lead);

        static string? BuildLeadName(DbLead dbLead)
        {
            if (string.IsNullOrEmpty(dbLead.CompanyName))
            {
                return dbLead.Subject;
            }

            var builder = new StringBuilder(dbLead.Subject);
            if (string.IsNullOrEmpty(dbLead.Subject) is false)
            {
                builder = builder.Append(' ');
            }

            return builder.Append('(').Append(dbLead.CompanyName).Append(')').ToString();
        }

        static string GetName(ProjectItem projectItem)
            =>
            projectItem.Name;
    }
}