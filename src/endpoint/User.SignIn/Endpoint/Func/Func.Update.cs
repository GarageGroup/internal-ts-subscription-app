using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class UserSignInFunc
{
    public ValueTask<Result<Unit, Failure<UserSignInFailureCode>>> InvokeAsync(
        UserSignInIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => SystemUserJson.BuildDataverseInput(@in.SystemUserId))
        .PipeValue(
            dataverseApi.GetEntityAsync<SystemUserJson>)
        .Map(
            user => new UserJson
            {
                BotId = option.BotId,
                BotName = $"{option.BotName} - {user.Value.FullName}",
                ChatId = input.ChatId,
                LanguageCode = DefaultLanguageCode,
                UserLookupValue = UserJson.BuildUserLookupValue(input.SystemUserId),
                IsSignedOut = false
            },
            static failure => failure.MapFailureCode(ToUserSignInFailureCode))
        .MapSuccess(
            user => UserJson.BuildDataverseInput(input.SystemUserId, option.BotId, user))
        .ForwardValue(
            dataverseApi.UpdateEntityAsync,
            static failure => failure.WithFailureCode(UserSignInFailureCode.Unknown));

    private static UserSignInFailureCode ToUserSignInFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => UserSignInFailureCode.SystemUserNotFound,
            _ => default
        };
}