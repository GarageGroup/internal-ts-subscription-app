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
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => DbProfile.QueryAll with
            {
                Top = 1,
                Filter = DbProfile.BuildDefaultFilter(@in.SystemUserId, option.BotId),
            })
        .PipeValue(
            sqlApi.QueryEntityOrFailureAsync<DbProfile>)
        .Map(
            MapProfileGetOut,
            static failure => failure.MapFailureCode(MapProfileGetFailureCode));

    private static ProfileGetOut MapProfileGetOut(DbProfile profile)
        =>
        new(
            userName: profile.UserName,
            languageCode: profile.LanguageCode);

    private static ProfileGetFailureCode MapProfileGetFailureCode(EntityQueryFailureCode failureCode)
        =>
        failureCode switch
        {
            EntityQueryFailureCode.NotFound => ProfileGetFailureCode.NotFound,
            _ => ProfileGetFailureCode.Unknown
        };
}