using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public sealed class SubscriptionDataJsonConverter : JsonConverter<BaseSubscriptionData>
{
    public override BaseSubscriptionData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);

        if (jsonDocument is null)
        {
            return null;
        }

        var notificationTypeElement = GetNotificationTypeJsonElement(jsonDocument, options);
        var notificationType = GetNotificationType(notificationTypeElement);

        return notificationType switch
        {
            NotificationType.DailyNotification => JsonSerializer.Deserialize<DailySubscriptionData>(jsonDocument, options),
            NotificationType.WeeklyNotification => JsonSerializer.Deserialize<WeeklySubscriptionData>(jsonDocument, options),
            _ => throw new JsonException($"Notification subscription data is unexpected")
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseSubscriptionData value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value is DailySubscriptionData dailySubscriptionData)
        {
            JsonSerializer.Serialize(writer, dailySubscriptionData, options);
            return;
        }

        if (value is WeeklySubscriptionData weeklySubscriptionData)
        {
            JsonSerializer.Serialize(writer, weeklySubscriptionData, options);
            return;
        }
        
        throw new NotSupportedException($"Type {value.GetType()} serialization is not supported");
    }
    
    private static JsonElement GetNotificationTypeJsonElement(JsonDocument jsonDocument, JsonSerializerOptions options)
    {
        var typePropertyName = nameof(BaseSubscriptionData.NotificationType);
        if (options.PropertyNamingPolicy is not null)
        {
            typePropertyName = options.PropertyNamingPolicy.ConvertName(typePropertyName);
        }

        if (jsonDocument.RootElement.TryGetProperty(typePropertyName, out var jsonElement))
        {
            return jsonElement;
        }

        throw new JsonException($"Property {typePropertyName} must be specified");
    }
    
    private static NotificationType GetNotificationType(JsonElement jsonElement)
    {
        if (jsonElement.ValueKind is JsonValueKind.Number)
        {
            return (NotificationType)jsonElement.GetInt32();
        }

        if (jsonElement.ValueKind is JsonValueKind.String)
        {
            var notificationType = jsonElement.GetString();
            return Enum.Parse<NotificationType>(notificationType, true);
        }

        throw new JsonException($"Notification type value kind {jsonElement.ValueKind} is unexpected");
    }
}