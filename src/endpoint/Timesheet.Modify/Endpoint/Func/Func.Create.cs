using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetModifyFunc
{
    public ValueTask<Result<Unit, Failure<TimesheetCreateFailureCode>>> InvokeAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.Project, cancellationToken)
        .PipeValue(
            GetProjectAsync)
        .Map(
            project => new TimesheetJson(project)
            {
                Date = input.Date,
                Description = input.Description.OrNullIfEmpty(),
                Duration = input.Duration,
                ChannelCode = TelegramChannelCode
            },
            static failure => failure.MapFailureCode(ToTimesheetCreateFailureCode))
        .MapSuccess(
            TimesheetJson.BuildDataverseCreateInput)
        .ForwardValue(
            dataverseApi.Impersonate(input.SystemUserId).CreateEntityAsync,
            static failure => failure.MapFailureCode(ToTimesheetCreateFailureCode));

    private static TimesheetCreateFailureCode ToTimesheetCreateFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => TimesheetCreateFailureCode.Forbidden,
            DataverseFailureCode.PrivilegeDenied => TimesheetCreateFailureCode.Forbidden,
            _ => default
        };

    private static TimesheetCreateFailureCode ToTimesheetCreateFailureCode(ProjectNameFailureCode failureCode)
        =>
        failureCode switch
        {
            ProjectNameFailureCode.ProjectNotFound => TimesheetCreateFailureCode.ProjectNotFound,
            ProjectNameFailureCode.InvalidProject => TimesheetCreateFailureCode.UnexpectedProjectType,
            _ => default
        };
}