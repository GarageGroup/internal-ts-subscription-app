using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class NotificationSubscriptionGetJson
{
    public static DataverseEntitySetGetIn BuildDataverseGetInput(Guid botUserId, Guid notificationTypeId)
        => new(
            entityPluralName: NotificationSubscriptionEntity.EntityPluralName,
            selectFields: [
                NotificationSubscriptionEntity.IdFieldName, 
                NotificationSubscriptionEntity.NotificationPreferencesFieldName, 
                NotificationSubscriptionEntity.NotificationTypeLookupName, 
                NotificationSubscriptionEntity.BotUserLookupName, 
                NotificationSubscriptionEntity.DisabledStatusFieldName
            ],
            filter: $@"{NotificationSubscriptionEntity.BotUserIdFieldName} eq '{botUserId}' 
                and {NotificationSubscriptionEntity.NotificationTypeIdFieldName} eq '{notificationTypeId}'",
            top:1
            );
    
    [JsonPropertyName(NotificationSubscriptionEntity.IdFieldName)]
    public Guid Id { get; init; }
    
    [JsonPropertyName($"{NotificationSubscriptionEntity.BotUserLookupName}@odata.bind")]
    public Guid BotUserId { get; init; }
    
    [JsonPropertyName($"{NotificationSubscriptionEntity.NotificationTypeLookupName}@odata.bind")]
    public Guid NotificationType { get; init; }
    
    [JsonPropertyName(NotificationSubscriptionEntity.DisabledStatusFieldName)]
    public bool? IsDisabled { get; init; }
    
    [JsonPropertyName(NotificationSubscriptionEntity.NotificationPreferencesFieldName)]
    public string? NotificationPreferences { get; init; }
}