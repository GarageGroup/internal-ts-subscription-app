using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class UserSignInFunc
{
    public ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        UserSignInIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => new UserJson
            {
                BotId = option.BotId,
                BotName = option.BotName,
                ChatId = @in.ChatId,
                LanguageCode = DefaultLanguageCode,
                UserLookupValue = UserJson.BuildUserLookupValue(@in.SystemUserId),
                IsSignedOut = false
            })
        .Pipe(
            static user => UserJson.BuildDataverseInput(user))
        .PipeValue(
            dataverseApi.CreateEntityAsync)
        .MapFailure(
            static failure => failure.WithFailureCode<Unit>(default));
}