using System;
using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc : INotificationSubscribeFunc
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    
    private readonly IDataverseApiClient dataverseApi;
    private readonly NotificationSubscribeOption option;

    internal NotificationSubscribeFunc(IDataverseApiClient dataverseApi, NotificationSubscribeOption option)
    {
        this.dataverseApi = dataverseApi;
        this.option = option;
    }

    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(NotificationSubscribeIn input)
    {
        if (input.SubscriptionData.UserPreference is null)
        {
            return new NotificationSubscriptionJson();
        }
        
        return input.SubscriptionData switch
        {
            DailyNotificationSubscriptionData dailySubscriptionData => ValidateAndMapToJsonDto(dailySubscriptionData.UserPreference),
            WeeklyNotificationSubscriptionData weeklySubscriptionData => ValidateAndMapToJsonDto(weeklySubscriptionData.UserPreference),
            _ => Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Unexpected type of user preferences")
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(DailyNotificationUserPreference? userPreference)
    {
        if (userPreference is null)
        {
            return new NotificationSubscriptionJson();
        }
        
        if (userPreference.WorkedHours <= 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Daily working hours cannot be less than zero");
        }

        var jsonUserPreferences = DailyNotificationUserPreferencesJson.Parse(userPreference);
        var userPreferences = JsonSerializer.Serialize(jsonUserPreferences, SerializerOptions);

        return new NotificationSubscriptionJson
        {
            UserPreferences = userPreferences
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(WeeklyNotificationUserPreference? userPreference)
    {
        if (userPreference is null)
        {
            return new NotificationSubscriptionJson();
        }
        
        if (userPreference.Weekday.IsEmpty)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Weekdays for notifications must be specified");
        }

        if (userPreference.WorkedHours <= 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Total week working hours cannot be less than zero");
        }
            
        var jsonUserPreferences = WeeklyNotificationUserPreferencesJson.Parse(userPreference);
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
            _ => Failure.Create(NotificationSubscribeFailureCode.NotificationTypeInvalid, "Not supported type of subscription data")
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
