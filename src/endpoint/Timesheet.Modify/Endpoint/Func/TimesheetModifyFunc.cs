using GarageGroup.Infra;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class TimesheetModifyFunc(IDataverseApiClient dataverseApi) : ITimesheetCreateFunc, ITimesheetUpdateFunc
{
    private const int TelegramChannelCode = 140120000;

    private ValueTask<Result<IProjectJson, Failure<ProjectNameFailureCode>>> GetProjectAsync(
        TimesheetProject input, CancellationToken cancellationToken)
        =>
        input.Type switch
        {
            ProjectType.Project => InnerGetProjectAsync<ProjectJson>(input.Id, cancellationToken),
            ProjectType.Incident => InnerGetProjectAsync<IncidentJson>(input.Id, cancellationToken),
            ProjectType.Opportunity => InnerGetProjectAsync<OpportunityJson>(input.Id, cancellationToken),
            ProjectType.Lead => InnerGetProjectAsync<LeadJson>(input.Id, cancellationToken),
            _ => new(Failure.Create(ProjectNameFailureCode.InvalidProject, $"An unexpected project type: {input.Type}"))
        };

    private ValueTask<Result<IProjectJson, Failure<ProjectNameFailureCode>>> InnerGetProjectAsync<TProjectJson>(
        Guid projectId, CancellationToken cancellationToken)
        where TProjectJson : IProjectJson, IProjectDataverseInputBuilder, new()
        =>
        AsyncPipeline.Pipe(
            projectId, cancellationToken)
        .Pipe(
            TProjectJson.BuildDataverseEntityGetIn)
        .PipeValue(
            dataverseApi.GetEntityAsync<TProjectJson>)
        .Map(
            static @out => (IProjectJson)(@out.Value ?? new()),
            static failure => failure.MapFailureCode(ToProjectNameFailureCode));

    private static ProjectNameFailureCode ToProjectNameFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => ProjectNameFailureCode.ProjectNotFound,
            _ => default
        };

    private enum ProjectNameFailureCode
    {
        Unknown,

        ProjectNotFound,

        InvalidProject
    }
}