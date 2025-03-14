using GarageGroup.Infra.Telegram.Bot;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Api.Subscription.BotApi.Test;

partial class BotApiSource
{
    public static TheoryData<BotUser, CacheValue> CacheSetTestData
        =>
        new()
        {
            {
                new(
                    id: 123456789,
                    isBot: false,
                    firstName: "John")
                {
                    LastName = "Doe",
                    Username = null,
                    LanguageCode = "en",
                    IsPremium = true,
                    CanJoinGroups = true,
                    CanReadAllGroupMessages = false,
                    SupportsInlineQueries = true
                },
                new()
                {
                    Id = 123456789,
                    Username = null
                }
            },
            {
                new(
                    id: 987654321,
                    isBot: true,
                    firstName: "SomeBotName")
                {
                    Username = "some_telegram_bot",
                    LanguageCode = "en",
                    CanJoinGroups = true,
                    CanReadAllGroupMessages = true,
                    SupportsInlineQueries = true
                },
                new()
                {
                    Id = 987654321,
                    Username = "some_telegram_bot"
                }
            }
        };
}