using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class ProfileGetFunc
{
    public ValueTask<Result<ProfileGetOut, Failure<ProfileGetFailureCode>>> InvokeAsync(
        ProfileGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe<Unit>(
            default, cancellationToken)
        .PipeValue(
            botApi.GetBotInfoAsync)
        .Map(
            bot => DbProfile.QueryAll with
            {
                Top = 1,
                Filter = DbProfile.BuildDefaultFilter(input.SystemUserId, bot.Id),
            },
            static failure => failure.WithFailureCode(ProfileGetFailureCode.Unknown))
        .ForwardValue(
            sqlApi.QueryEntityOrFailureAsync<DbProfile>,
            static failure => failure.MapFailureCode(MapFailureCode))
        .MapSuccess(
            static profile => new ProfileGetOut(
                userName: profile.UserName,
                languageCode: profile.LanguageCode));

    private static ProfileGetFailureCode MapFailureCode(EntityQueryFailureCode failureCode)
        =>
        failureCode switch
        {
            EntityQueryFailureCode.NotFound => ProfileGetFailureCode.NotFound,
            _ => ProfileGetFailureCode.Unknown
        };
}