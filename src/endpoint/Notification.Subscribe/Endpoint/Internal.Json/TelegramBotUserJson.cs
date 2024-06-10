using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal readonly record struct TelegramBotUserJson
{
    private const string EntityPluralName = "gg_telegram_bot_users";

    private const string IdFieldName = "gg_telegram_bot_userid"; 

    private const string BotIdFieldName = "gg_bot_id";

    private const string SystemUserIdFieldName = "_gg_systemuser_id_value";

    private static readonly FlatArray<string> SelectedFields = [IdFieldName];

    public static DataverseEntityGetIn BuildGetInput(Guid systemUserId, long botId)
        =>
        new(
            entityPluralName: EntityPluralName,
            selectFields: SelectedFields,
            entityKey: new DataverseAlternateKey(
                [
                    new(SystemUserIdFieldName, $"{systemUserId}"),
                    new(BotIdFieldName, $"'{botId}'")
                ]));
    
    
    [JsonPropertyName(IdFieldName)]
    public Guid Id { get; init; }
}