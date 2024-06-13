using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class ProfileJson
{
    private const string EntityPluralName = "gg_telegram_bot_users";

    private const string BotIdFieldName = "gg_bot_id";

    private const string SystemUserIdFieldName = "_gg_systemuser_id_value";

    internal static DataverseEntityUpdateIn<ProfileJson> BuildDataverseInput(Guid systemUserId, long botId, ProfileJson profile)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityData: profile,
            entityKey: new DataverseAlternateKey(
                [
                    new(SystemUserIdFieldName, $"{systemUserId}"),
                    new(BotIdFieldName, $"'{botId}'")
                ]));

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_language_code")]
    public string? LanguageCode { get; init; }
}