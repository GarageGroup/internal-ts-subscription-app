using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

partial class SubscriptionSetGetFuncSource
{
    public static TheoryData<FlatArray<SubscriptionJson>, SubscriptionSetGetOut> OutputValidTestData
        => 
        new()
        {
            {
                default,
                default
            },
            {
                [
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        }
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "WeeklyTimesheetNotification"
                        }
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        }
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        }
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "DailyTimesheetNotification"
                        },
                        UserPreference = """{"workedHours":8,"flowRuntime":"18:00"}"""
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        },
                        UserPreference = """{"weekday":"6,,0","workedHours":40,"flowRuntime":"20:00"}"""
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        },
                        UserPreference = """{"workedHours":4.15,"flowRuntime":"19:00"}"""
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        },
                        UserPreference = """{"weekday":"5","workedHours":20.5,"flowRuntime":"18:00"}"""
                    },
                    new()
                    {
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        },
                        UserPreference = """{"weekday":null,"workedHours":40}"""
                    }
                ],
                new SubscriptionSetGetOut
                {
                    Subscriptions =
                    [
                        new DailyNotificationSubscription(
                            userPreference: null),
                        new WeeklyNotificationSubscription(
                            userPreference: null),
                        new DailyNotificationSubscription(
                            userPreference: null),
                        new WeeklyNotificationSubscription(
                            userPreference: null),
                        new DailyNotificationSubscription(
                            userPreference: new(
                                workedHours: 8,
                                notificationTime: "18:00")),
                        new WeeklyNotificationSubscription(
                            userPreference: new(
                                weekday: [Weekday.Saturday, Weekday.Sunday],
                                notificationTime: "20:00",
                                workedHours: 40)),
                        new DailyNotificationSubscription(
                            userPreference: new(
                                workedHours: 4.15m,
                                notificationTime: "19:00")),
                        new WeeklyNotificationSubscription(
                            userPreference: new(
                                weekday: [Weekday.Friday],
                                notificationTime: "18:00",
                                workedHours: 20.5m)),
                        new WeeklyNotificationSubscription(
                            userPreference: new(
                                weekday: default,
                                notificationTime: null,
                                workedHours: 40)),
                    ]
                }
            }
        };
}