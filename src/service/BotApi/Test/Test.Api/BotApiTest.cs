using System;
using System.Threading;
using GarageGroup.Infra.Telegram.Bot;
using Moq;

namespace GarageGroup.Internal.Timesheet.Api.Subscription.BotApi.Test;

public static partial class BotApiTest
{
    private static readonly CacheValue SomeCacheValue
        =
        new()
        {
            Id = 987654321,
            Username = "some_telegram_bot"
        };

    private static readonly BotUser SomeBotUser
        =
        new(
            id: 6903100931,
            isBot: true,
            firstName: "SomeBotApp")
        {
            Username = "SomeAppBot"
        };

    private static Mock<IBotUserApiSupplier> BuildMockTelegramApi(
        in Result<BotUser, Failure<TelegramBotFailureCode>> result)
    {
        var mock = new Mock<IBotUserApiSupplier>();

        _ = mock.Setup(static a => a.GetMeAsync(It.IsAny<Unit>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        return mock;
    }

    private static Mock<ICacheApi> BuildMockCacheApi(
        in CacheValue? cacheValue)
    {
        var mock = new Mock<ICacheApi>();

        _ = mock.Setup(static a => a.GetValue()).Returns(cacheValue);
        _ = mock.Setup(static a => a.SetValue(It.IsAny<CacheValue>())).Returns(default(Unit));

        return mock;
    }
}