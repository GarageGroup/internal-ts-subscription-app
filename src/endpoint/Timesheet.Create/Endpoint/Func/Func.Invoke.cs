using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetCreateFunc
{
    public ValueTask<Result<Unit, Failure<TimesheetCreateFailureCode>>> InvokeAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            BuildTimesheetJsonOrFailure)
        .MapSuccess(
            TimesheetJson.BuildDataverseCreateInput)
        .ForwardValue(
            dataverseApi.Impersonate(input.SystemUserId).CreateEntityAsync,
            static failure => failure.MapFailureCode(ToTimesheetCreateFailureCode));

    private Result<TimesheetJson, Failure<TimesheetCreateFailureCode>> BuildTimesheetJsonOrFailure(TimesheetCreateIn input)
    {
        var timesheet = new TimesheetJson
        {
            Subject = input.Project.DisplayName,
            Date = input.Date,
            Description = input.Description.OrNullIfEmpty(),
            Duration = input.Duration,
            ChannelCode = (int)ChannelCode.Telegram
        };

        return BindProjectOrFailure(timesheet, input.Project);
    }

    private static TimesheetCreateFailureCode ToTimesheetCreateFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => TimesheetCreateFailureCode.Forbidden,
            DataverseFailureCode.PrivilegeDenied => TimesheetCreateFailureCode.Forbidden,
            _ => default
        };
}