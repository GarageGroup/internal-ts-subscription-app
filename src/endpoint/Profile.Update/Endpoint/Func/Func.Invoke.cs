using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProfileUpdateFunc
{
    public ValueTask<Result<Unit, Failure<ProfileUpdateFailureCode>>> InvokeAsync(
        ProfileUpdateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe<Unit>(
            default, cancellationToken)
        .PipeValue(
            botApi.GetBotInfoAsync)
        .Map(
            bot => ProfileJson.BuildDataverseInput(
                systemUserId: input.SystemUserId,
                botId: bot.Id,
                profile: new()
                {
                    LanguageCode = input.LanguageCode?.Code
                }),
            static failure => failure.WithFailureCode(ProfileUpdateFailureCode.Unknown))
        .ForwardValue(
            dataverseApi.UpdateEntityAsync,
            static failure => failure.MapFailureCode(MapFailureCode));

    private static ProfileUpdateFailureCode MapFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => ProfileUpdateFailureCode.NotFound,
            _ => ProfileUpdateFailureCode.Unknown
        };
}