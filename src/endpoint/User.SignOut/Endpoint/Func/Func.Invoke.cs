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
        AsyncPipeline.Pipe<Unit>(
            default, cancellationToken)
        .PipeValue(
            botApi.GetBotInfoAsync)
        .MapSuccess(
            bot => UserJson.BuildDataverseInput(
                systemUserId: input.SystemUserId,
                botId: bot.Id,
                user: new()
                {
                    IsSignedOut = true
                }))
        .ForwardValue(
            UpdateAsync);

    private ValueTask<Result<Unit, Failure<Unit>>> UpdateAsync(
        DataverseEntityUpdateIn<UserJson> input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            dataverseApi.UpdateEntityAsync)
        .Recover(
            MapFailureOrRecover);

    private static Result<Unit, Failure<Unit>> MapFailureOrRecover(Failure<DataverseFailureCode> failure)
        =>
        failure.FailureCode switch
        {
            DataverseFailureCode.RecordNotFound => Result.Success<Unit>(default),
            _ => failure.WithFailureCode<Unit>(default)
        };
}
