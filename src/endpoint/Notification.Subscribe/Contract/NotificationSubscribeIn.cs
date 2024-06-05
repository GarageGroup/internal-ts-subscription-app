using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class NotificationSubscribeIn
{
    public NotificationSubscribeIn(
        [JsonBodyIn] long botId,
        [JsonBodyIn] long chatId, 
        [JsonBodyIn] string? notificationType,
        [JsonBodyIn] JsonElement? userPreferenceJson)
    {
        BotId = botId;
        ChatId = chatId;
        NotificationType = notificationType ?? string.Empty;
        UserPreferenceJson = userPreferenceJson;
    }
    
    public long BotId { get; }

    public long ChatId { get; }

    public string NotificationType { get; }

    public JsonElement? UserPreferenceJson { get; }
}