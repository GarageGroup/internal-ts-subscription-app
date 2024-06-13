using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class UserJson
{
    private const string EntityPluralName = "gg_telegram_bot_users";

    private const string BotIdFieldName = "gg_bot_id";

    private const string SystemUserIdFieldName = "_gg_systemuser_id_value";

    internal static DataverseEntityUpdateIn<UserJson> BuildDataverseInput(Guid systemUserId, long botId, UserJson user)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityData: user,
            entityKey: new DataverseAlternateKey(
                [
                    new(SystemUserIdFieldName, $"{systemUserId}"),
                    new(BotIdFieldName, $"'{botId}'")
                ]));

    [JsonPropertyName("gg_is_user_signed_out")]
    public bool IsSignedOut { get; init; }
}