using System;
using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc : INotificationSubscribeFunc
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    
    private readonly IDataverseApiClient dataverseApi;
    
    internal NotificationSubscribeFunc(IDataverseApiClient dataverseApi) 
        => this.dataverseApi = dataverseApi;

    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(NotificationSubscribeIn input)
    {
        if (input.SubscriptionData.UserPreference is null)
        {
            return new NotificationSubscriptionJson
            {
                UserPreferences = string.Empty
            };
        }
        
        return input.SubscriptionData switch
        {
            DailyNotificationSubscriptionData dailySubscriptionData => ValidateAndMapToJsonDto(dailySubscriptionData),
            WeeklyNotificationSubscriptionData dailySubscriptionData => ValidateAndMapToJsonDto(dailySubscriptionData),
            _ => Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Unexpected type of user preferences")
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(DailyNotificationSubscriptionData dailySubscriptionData)
    {
        ArgumentNullException.ThrowIfNull(dailySubscriptionData);
        ArgumentNullException.ThrowIfNull(dailySubscriptionData.UserPreference);
        
        if (dailySubscriptionData.UserPreference.WorkedHours < 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Daily working hours cannot be less than zero");
        }

        var jsonUserPreferences = DailyNotificationUserPreferencesJson.Parse(dailySubscriptionData.UserPreference);
        var userPreferences = JsonSerializer.Serialize(jsonUserPreferences, SerializerOptions);

        return new NotificationSubscriptionJson
        {
            UserPreferences = userPreferences
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(WeeklyNotificationSubscriptionData weeklyNotificationData)
    {
        ArgumentNullException.ThrowIfNull(weeklyNotificationData);
        ArgumentNullException.ThrowIfNull(weeklyNotificationData.UserPreference);
        
        if (weeklyNotificationData.UserPreference.Weekday.IsEmpty)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Weekdays for notifications must be specified");
        }

        if (weeklyNotificationData.UserPreference.WorkedHours < 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Total week working hours cannot be less than zero");
        }
            
        var jsonUserPreferences = WeeklyNotificationUserPreferencesJson.Parse(weeklyNotificationData.UserPreference);
        var userPreferences = JsonSerializer.Serialize(jsonUserPreferences, SerializerOptions);

        return new NotificationSubscriptionJson
        {
            UserPreferences = userPreferences
        };
    }
    
    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingBotUser(DataverseFailureCode failureCode) 
        => 
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.BotUserNotFound,
            _ => NotificationSubscribeFailureCode.Unknown 
        };
    
    private static Result<string, Failure<NotificationSubscribeFailureCode>> MapToNotificationTypeKey(NotificationSubscribeIn input) 
        => 
        input.SubscriptionData switch
        {
            DailyNotificationSubscriptionData => "dailyTimesheetNotification",
            WeeklyNotificationSubscriptionData => "weeklyTimesheetNotification",
            _ => throw new NotSupportedException("Not supported type of subscription data")
        };
    
    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingNotificationType(DataverseFailureCode code)
        => 
        code switch
        { 
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.NotificationTypeNotFound, 
            _ => NotificationSubscribeFailureCode.Unknown
        };
    
    private sealed record class NotificationData(NotificationSubscribeIn Input, NotificationSubscriptionJson Subscription);
}