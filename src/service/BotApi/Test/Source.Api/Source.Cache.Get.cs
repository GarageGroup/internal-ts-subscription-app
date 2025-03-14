using Xunit;

namespace GarageGroup.Internal.Timesheet.Api.Subscription.BotApi.Test;

partial class BotApiSource
{
    public static TheoryData<CacheValue, BotInfoGetOut> CacheGetTestData
        =>
        new()
        {
            {
                new()
                {
                    Id = 7961237012,
                    Username = null
                },
                new(
                    id: 7961237012,
                    username: string.Empty)
            },
            {
                new()
                {
                    Id = 8917241284,
                    Username = "SomeBot"
                },
                new(
                    id: 8917241284,
                    username: "SomeBot")
            }
        };
}