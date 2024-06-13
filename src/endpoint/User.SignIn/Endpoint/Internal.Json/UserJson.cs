using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class UserJson
{
    private const string EntityPluralName = "gg_telegram_bot_users";

    internal static DataverseEntityCreateIn<UserJson> BuildDataverseInput(UserJson user)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityData: user);

    internal static string BuildUserLookupValue(Guid systemUserId)
        =>
        $"/systemusers({systemUserId:D})";

    [JsonPropertyName("gg_systemuser_id@odata.bind")]
    public string? UserLookupValue { get; init; }

    [JsonPropertyName("gg_is_user_signed_out")]
    public bool IsSignedOut { get; init; }

    [JsonPropertyName("gg_bot_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public long BotId { get; init; }

    [JsonPropertyName("gg_chat_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public long ChatId { get; init; }

    [JsonPropertyName("gg_name")]
    public string? BotName { get; init; }

    [JsonPropertyName("gg_language_code")]
    public string? LanguageCode { get; init; }
}