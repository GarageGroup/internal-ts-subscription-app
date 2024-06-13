using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class UserSignOutFunc
{
    public ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        UserSignOutIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new UserJson()
            {
                IsSignedOut = true
            })
        .Pipe(
            user => UserJson.BuildDataverseInput(input.SystemUserId, option.BotId, user))
        .PipeValue(
            dataverseApi.UpdateEntityAsync)
        .Recover(
            MapFailure);

    private static Result<Unit, Failure<Unit>> MapFailure(Failure<DataverseFailureCode> failure)
        =>
        failure.FailureCode switch
        {
            DataverseFailureCode.RecordNotFound => Result.Success<Unit>(default),
            _ => failure.WithFailureCode<Unit>(default)
        };
}