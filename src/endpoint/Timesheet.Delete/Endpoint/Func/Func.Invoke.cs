using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetDeleteFunc
{
    public ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        TimesheetDeleteIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.TimesheetId, cancellationToken)
        .Pipe(
            TimesheetJson.BuildDataverseDeleteInput)
        .PipeValue(
            dataverseApi.DeleteEntityAsync)
        .Recover(
            MapFailure);

    private static Result<Unit, Failure<Unit>> MapFailure(Failure<DataverseFailureCode> failure)
        =>
        failure.FailureCode switch
        {
            DataverseFailureCode.RecordNotFound => default(Unit),
            _ => failure.WithFailureCode<Unit>(default)
        };
}