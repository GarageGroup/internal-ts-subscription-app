using System;
using System.Text.Json;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Inner.Json;
using GarageGroup.Internal.Timesheet.Option;

namespace GarageGroup.Internal.Timesheet;

public sealed partial class SubscriptionSetGetFunc : ISubscriptionSetGetFunc
{
    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    private readonly IDataverseApiClient dataverseApi;
    private readonly SubscriptionSetGetOption option;

    public SubscriptionSetGetFunc(IDataverseApiClient dataverseApi, SubscriptionSetGetOption option)
    {
        this.dataverseApi = dataverseApi;
        this.option = option;
    }

    private static SubscriptionSetGetFailureCode MapFailureCodeWhenSearchForUser(DataverseFailureCode failureCode)
        => 
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => SubscriptionSetGetFailureCode.BotUserNotFound,
            _ => SubscriptionSetGetFailureCode.Unknown
        };

    private static SubscriptionDto MapToSubscriptionDto(SubscriptionJson subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription.NotificationType);

        return subscription.NotificationType.Key switch
        {
            "dailyTimesheetNotification" => MapToDailySubscriptionDto(subscription),
            "weeklyTimesheetNotification" => MapWeeklySubscriptionDto(subscription),
            _ => throw new NotSupportedException("Not supported notification type key")
        };
    }

    private static SubscriptionDto MapToDailySubscriptionDto(SubscriptionJson subscription)
        => 
        new()
        {
            Id = subscription.Id, 
            NotificationType = NotificationType.DailyNotification, 
            IsActive = subscription.IsActive, 
            UserPreference = string.IsNullOrEmpty(subscription.UserPreference) is false
                ? JsonSerializer.Deserialize<DailyNotificationUserPreferenceDto>(subscription.UserPreference, SerializerOptions)
                : null
        };

    private static SubscriptionDto MapWeeklySubscriptionDto(SubscriptionJson subscription)
        => 
        new() 
        { 
            Id = subscription.Id, 
            NotificationType = NotificationType.WeeklyNotification, 
            IsActive = subscription.IsActive, 
            UserPreference = string.IsNullOrEmpty(subscription.UserPreference) is false 
                ? JsonSerializer.Deserialize<WeeklyNotificationUserPreferenceDtoDto>(subscription.UserPreference, SerializerOptions)
                : null
        };

    private static Subscription MapToSubscription(SubscriptionDto subscription)
        => 
        new() 
        { 
            Id = subscription.Id, 
            NotificationType = subscription.NotificationType, 
            IsActive = subscription.IsActive, 
            UserPreference = Map(subscription.UserPreference)
        };

    private static INotificationUserPreference? Map(INotificationUserPreferenceDto? userPreference)
        => 
        userPreference switch
        {
            DailyNotificationUserPreferenceDto dailyPreference 
                => 
                new DailyNotificationUserPreference
                {
                    WorkedHours = dailyPreference.WorkedHours,
                    NotificationTime = dailyPreference.NotificationTime
                },
            
            WeeklyNotificationUserPreferenceDtoDto weeklyPreference 
                => 
                new WeeklyNotificationUserPreference
                {
                    NotificationTime = weeklyPreference.NotificationTime,
                    WorkedHours = weeklyPreference.WorkedHours,
                    Weekday = weeklyPreference.Weekday
                },

            null => null,

            _ => throw new NotSupportedException()
        };
}