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
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new ProfileJson()
            {
                LanguageCode = @in.LanguageCode
            })
        .Pipe(
            profile => ProfileJson.BuildDataverseInput(input.SystemUserId, option.BotId, profile))
        .PipeValue(
            dataverseApi.UpdateEntityAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapProfileUpdateFailureCode));

    private static ProfileUpdateFailureCode MapProfileUpdateFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => ProfileUpdateFailureCode.NotFound,
            _ => ProfileUpdateFailureCode.Unknown
        };
}