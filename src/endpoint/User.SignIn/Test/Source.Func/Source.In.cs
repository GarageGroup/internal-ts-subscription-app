using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.User.SignIn.Test;

partial class UserSignInFuncSource
{
    public static TheoryData<UserSignInOption, UserSignInIn, DataverseEntityGetOut<SystemUserJson>, DataverseEntityUpdateIn<UserJson>> InputTestData
        =>
        new()
        {
            {
                new()
                {
                    BotId = 123123,
                    BotName = "Some bot name"
                },
                new(
                    systemUserId: new("2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                    chatId: 123123123),
                new(
                    value: new()
                    {
                        FullName = "Some user name"
                    }),
                new(
                    entityPluralName: "gg_telegram_bot_users",
                    entityKey: new DataverseAlternateKey(
                    [
                        new("_gg_systemuser_id_value", "2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                        new("gg_bot_id", "'123123'")
                    ]),
                    entityData: new UserJson
                    {
                        BotId = 123123,
                        BotName = "Some bot name - Some user name",
                        ChatId = 123123123,
                        LanguageCode = "en",
                        UserLookupValue = $"/systemusers(2e159d1a-42ac-4c9f-af70-b094ba32a786)",
                        IsSignedOut = false,
                    })
                {
                    OperationType = DataverseUpdateOperationType.Upsert
                }
            }
        };
}
