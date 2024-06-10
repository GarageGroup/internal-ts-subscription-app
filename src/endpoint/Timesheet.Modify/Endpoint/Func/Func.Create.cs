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
            project => (input, project),
            static failure => failure.MapFailureCode(ToTimesheetCreateFailureCode))
        .MapSuccess(
            BuildTimesheetJson)
        .MapSuccess(
            TimesheetJson.BuildDataverseCreateInput)
        .ForwardValue(
            dataverseApi.Impersonate(input.SystemUserId).CreateEntityAsync,
            static failure => failure.MapFailureCode(ToTimesheetCreateFailureCode));

    private TimesheetJson BuildTimesheetJson((TimesheetCreateIn Input, IProjectJson Project) input)
    {
        var timesheet = new TimesheetJson
        {
            Subject = input.Project.GetName(),
            Date = input.Input.Date,
            Description = input.Input.Description.OrNullIfEmpty(),
            Duration = input.Input.Duration,
            ChannelCode = (int)ChannelCode.Telegram
        };

        return BindProject(timesheet, input.Project, input.Input.Project.Type);
    }

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