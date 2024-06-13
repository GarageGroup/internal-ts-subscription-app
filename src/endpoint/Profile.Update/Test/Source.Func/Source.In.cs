using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.Update.Test;

partial class ProfileUpdateFuncSource
{
    public static TheoryData<ProfileUpdateOption, ProfileUpdateIn, DataverseEntityUpdateIn<ProfileJson>> InputTestData
        =>
        new()
        {
            {
                new()
                {
                    BotId = 123123,
                },
                new(
                    systemUserId: new("2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                    languageCode: ProfileLanguage.English),
                new (
                    entityPluralName: "gg_telegram_bot_users",
                    entityData: new ProfileJson()
                    {
                        LanguageCode = "en",
                    },
                    entityKey: new DataverseAlternateKey(
                    [
                        new("_gg_systemuser_id_value", "2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                        new("gg_bot_id", "'123123'")
                    ]))
            },
            {
                new()
                {
                    BotId = 1222333,
                },
                new(
                    systemUserId: new("93dba5ba-2506-4337-a040-be612612a161"),
                    languageCode: ProfileLanguage.Russian),
                new (
                    entityPluralName: "gg_telegram_bot_users",
                    entityData: new ProfileJson()
                    {
                        LanguageCode = "ru",
                    },
                    entityKey: new DataverseAlternateKey(
                    [
                        new("_gg_systemuser_id_value", "93dba5ba-2506-4337-a040-be612612a161"),
                        new("gg_bot_id", "'1222333'")
                    ]))
            }
        };
}