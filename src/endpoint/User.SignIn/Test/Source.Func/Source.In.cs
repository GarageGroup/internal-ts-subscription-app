using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.User.SignIn.Test;

partial class UserSignInFuncSource
{
    public static TheoryData<UserSignInOption, UserSignInIn, DataverseEntityCreateIn<UserJson>> InputTestData
        =>
        new()
        {
            {
                new()
                {
                    BotId = 123123,
                    BotName = "Test"
                },
                new(
                    systemUserId: new("2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                    chatId: 123123123),
                new (
                    entityPluralName: "gg_telegram_bot_users",
                    entityData: new UserJson
                    {
                        BotId = 123123,
                        BotName = "Test",
                        ChatId = 123123123,
                        LanguageCode = "en",
                        UserLookupValue = $"/systemusers(2e159d1a-42ac-4c9f-af70-b094ba32a786)",
                        IsSignedOut = false,
                    })
            }
        };
}
