using System;
using System.Linq;
using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc : INotificationSubscribeFunc
{
    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    private readonly IDataverseApiClient dataverseApi;

    private readonly NotificationSubscribeOption option;

    internal NotificationSubscribeFunc(IDataverseApiClient dataverseApi, NotificationSubscribeOption option)
    {
        this.dataverseApi = dataverseApi;
        this.option = option;
    }

    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(
        NotificationSubscribeIn input)
    {
        if (input.SubscriptionData.UserPreference is null)
        {
            return new NotificationSubscriptionJson();
        }
        
        return input.SubscriptionData.UserPreference switch
        {
            DailyNotificationUserPreference userPreference => ValidateAndMapToJsonDto(userPreference),
            WeeklyNotificationUserPreference userPreference => ValidateAndMapToJsonDto(userPreference),
            _ => Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Unexpected type of user preferences")
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(
        DailyNotificationUserPreference userPreference)
    {
        if (userPreference.WorkedHours <= 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Daily working hours cannot be less than zero");
        }

        var userPreferencesJson = new DailyNotificationUserPreferencesJson
        {
            WorkedHours = userPreference.WorkedHours,
            FlowRuntime = userPreference.NotificationTime.Time.ToString("HH:mm")
        };

        return new NotificationSubscriptionJson
        {
            UserPreferences = JsonSerializer.Serialize(userPreferencesJson, SerializerOptions)
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(
        WeeklyNotificationUserPreference userPreference)
    {
        if (userPreference.Weekday.IsEmpty)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Weekdays for notifications must be specified");
        }

        if (userPreference.WorkedHours <= 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Total week working hours cannot be less than zero");
        }

        var userPreferencesJson = new WeeklyNotificationUserPreferencesJson
        {
            Weekday = string.Join(',', userPreference.Weekday.AsEnumerable().Select(AsInt32)),
            WorkedHours = userPreference.WorkedHours,
            FlowRuntime = userPreference.NotificationTime.Time.ToString("HH:mm")
        };

        return new NotificationSubscriptionJson
        {
            UserPreferences = JsonSerializer.Serialize(userPreferencesJson, SerializerOptions)
        };

        static int AsInt32(Weekday weekday)
            =>
            (int)weekday;
    }

    private static Result<string, Failure<NotificationSubscribeFailureCode>> MapToNotificationTypeKey(NotificationSubscribeIn input) 
        => 
        input.SubscriptionData switch
        {
            DailyNotificationSubscriptionData => "dailyTimesheetNotification",
            WeeklyNotificationSubscriptionData => "weeklyTimesheetNotification",
            _ => Failure.Create(NotificationSubscribeFailureCode.NotificationTypeInvalid, "Not supported type of subscription data")
        };

    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingBotUser(DataverseFailureCode failureCode) 
        => 
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.BotUserNotFound,
            _ => NotificationSubscribeFailureCode.Unknown 
        };

    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingNotificationType(DataverseFailureCode failureCode)
        => 
        failureCode switch
        { 
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.NotificationTypeNotFound, 
            _ => NotificationSubscribeFailureCode.Unknown
        };

    private sealed record class NotificationData(NotificationSubscribeIn Input, NotificationSubscriptionJson Subscription);
}
