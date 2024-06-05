using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class NotificationTypeJson
{
    public const string EntityPluralName = "gg_bot_notification_types";

    private const string IdFieldName = "gg_bot_notification_typeid";

    private const string BotIdFieldName = "_gg_telegrambot_id_value";
    
    private const string KeyFieldName = "gg_key";

    public static DataverseEntitySetGetIn BuildGetInput(string typeKey)
        => new (
            entityPluralName: EntityPluralName,
            selectFields: [IdFieldName],
            top: 1, 
            filter: $"{KeyFieldName} eq '{typeKey}'");
    
    [JsonPropertyName(IdFieldName)]
    public Guid Id { get; init; }
}