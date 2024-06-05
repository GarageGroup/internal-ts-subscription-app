using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class TelegramBotUserJson
{
    public const string EntityPluralName = "gg_telegram_bot_users";

    private const string IdFieldName = "gg_telegram_bot_userid"; 

    private const string BotIdFieldName = "gg_bot_id";

    private const string ChatIdFieldName = "gg_chat_id";

    public static DataverseEntityGetIn BuildGetInput(long botId, long chatId)
        => new(
            entityPluralName: EntityPluralName,
            selectFields: [IdFieldName],
            entityKey: new DataverseAlternateKey(new KeyValuePair<string, string>[]
                {
                    new(BotIdFieldName, $"'{botId}'"),
                    new(ChatIdFieldName, $"'{chatId}'")
                })
            );
    
    
    [JsonPropertyName(IdFieldName)]
    public Guid Id { get; init; }    
}