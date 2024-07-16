using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.User.SignOut.Test;

partial class UserSignOutFuncSource
{
    public static TheoryData<BotInfoGetOut, UserSignOutIn, DataverseEntityUpdateIn<UserJson>> InputTestData
        =>
        new()
        {
            {
                new(
                    id: 12409812904,
                    username: "SomeBot"),
                new(
                    systemUserId: new("2e159d1a-42ac-4c9f-af70-b094ba32a786")),
                new (
                    entityPluralName: "gg_telegram_bot_users",
                    entityData: new UserJson
                    {
                        IsSignedOut = true,
                    },
                    entityKey: new DataverseAlternateKey(
                    [
                        new("_gg_systemuser_id_value", "2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                        new("gg_bot_id", "'12409812904'")
                    ]))
            }
        };
}