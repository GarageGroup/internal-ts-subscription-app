using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class BotApiImpl
{
    public ValueTask<Result<BotInfoGetOut, Failure<Unit>>> GetBotInfoAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            GetCacheValueAsync)
        .MapSuccess(
            static cache => new BotInfoGetOut(
                id: cache.Id,
                username: cache.Username));

    private ValueTask<Result<CacheValue, Failure<Unit>>> GetCacheValueAsync(
        Unit input, CancellationToken cancellationToken)
    {
        var cacheValue = cacheApi.GetValue();
        if (cacheValue is not null)
        {
            return new(cacheValue);
        }

        return UpdateCacheValueAsync(input, cancellationToken);
    }

    private ValueTask<Result<CacheValue, Failure<Unit>>> UpdateCacheValueAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            telegramApi.GetMeAsync)
        .Map(
            static bot => new CacheValue
            {
                Id = bot.Id,
                Username = bot.Username
            },
            static failure => failure.WithFailureCode<Unit>(default))
        .OnSuccess(
            cacheApi.SetValue);
}