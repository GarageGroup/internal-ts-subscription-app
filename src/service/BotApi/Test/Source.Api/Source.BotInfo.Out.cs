using GarageGroup.Infra.Telegram.Bot;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Service.BotApi.Test;

partial class BotApiSource
{
    public static TheoryData<BotUser, BotInfoGetOut> BotInfoOutTestData
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
                new(
                    id: 123456789,
                    username: string.Empty)
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
                new(
                    id: 987654321,
                    username: "some_telegram_bot")
            }
        };
}