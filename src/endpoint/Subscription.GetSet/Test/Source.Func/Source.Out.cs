using System;
using Xunit;
using GarageGroup.Internal.Timesheet.Inner.Json;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

partial class SubscriptionSetGetFuncSource
{
    public static TheoryData<FlatArray<SubscriptionJson>, FlatArray<Timesheet.Subscription>> OutputData
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
                        Id = Guid.Parse("1215859e-dad7-4d6a-85c1-560156ad4a1b"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        }
                    },
                    new()
                    {
                        Id = Guid.Parse("2d52d9de-e066-4c9c-a906-c40188ec0748"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        }
                    },
                    new()
                    {
                        Id = Guid.Parse("1ad594b1-0fe4-4f15-b74d-ad6a0a6cde4b"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        },
                        IsDisabled = true
                    },
                    new()
                    {
                        Id = Guid.Parse("a60c2236-885a-4230-a678-16f79b2783ef"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        },
                        IsDisabled = true
                    },
                    new()
                    {
                        Id = Guid.Parse("7b06e225-2097-4bca-84bd-8a6942515822"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        },
                        UserPreference = """{"workedHours":8,"flowRuntime":"18:00"}"""
                    },
                    new()
                    {
                        Id = Guid.Parse("ce609ea1-5206-4d89-b418-727557ba3367"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        },
                        UserPreference = """{"weekday":"6,0","workedHours":40,"flowRuntime":"20:00"}"""
                    },
                    new()
                    {
                        Id = Guid.Parse("6c78ed32-2c2e-48ad-b6d6-73bf0b1a8980"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "dailyTimesheetNotification"
                        },
                        UserPreference = """{"workedHours":4,"flowRuntime":"19:00"}""",
                        IsDisabled = true
                    },
                    new()
                    {
                        Id = Guid.Parse("66ba83e9-f3a7-4733-9a40-9a62ddbcc12e"),
                        NotificationType = new NotificationTypeJson
                        {
                            Key = "weeklyTimesheetNotification"
                        },
                        UserPreference = """{"weekday":"5","workedHours":20,"flowRuntime":"18:00"}""",
                        IsDisabled = true
                    }
                ],
                [
                    new()
                    {
                        Id = Guid.Parse("1215859e-dad7-4d6a-85c1-560156ad4a1b"),
                        NotificationType = NotificationType.DailyNotification,
                    },
                    new()
                    {
                        Id = Guid.Parse("2d52d9de-e066-4c9c-a906-c40188ec0748"),
                        NotificationType = NotificationType.WeeklyNotification,
                    },
                    new()
                    {
                        Id = Guid.Parse("1ad594b1-0fe4-4f15-b74d-ad6a0a6cde4b"),
                        NotificationType = NotificationType.DailyNotification,
                        IsDisabled = true
                    },
                    new()
                    {
                        Id = Guid.Parse("a60c2236-885a-4230-a678-16f79b2783ef"),
                        NotificationType = NotificationType.WeeklyNotification,
                        IsDisabled = true
                    },
                    new()
                    {
                        Id = Guid.Parse("7b06e225-2097-4bca-84bd-8a6942515822"),
                        NotificationType = NotificationType.DailyNotification,
                        UserPreference = new DailyNotificationUserPreference
                        {
                            WorkedHours = 8,
                            NotificationTime = new TimeOnly(18, 0),
                        }
                    },
                    new()
                    {
                        Id = Guid.Parse("ce609ea1-5206-4d89-b418-727557ba3367"),
                        NotificationType = NotificationType.WeeklyNotification,
                        UserPreference = new WeeklyNotificationUserPreference
                        {
                            Weekday = [Weekday.Saturday, Weekday.Sunday],
                            NotificationTime = new TimeOnly(20, 0),
                            WorkedHours = 40
                        }
                    },
                    new()
                    {
                        Id = Guid.Parse("6c78ed32-2c2e-48ad-b6d6-73bf0b1a8980"),
                        NotificationType = NotificationType.DailyNotification,
                        UserPreference = new DailyNotificationUserPreference
                        {
                            WorkedHours = 4,
                            NotificationTime = new TimeOnly(19, 0),
                        },
                        IsDisabled = true
                    },
                    new()
                    {
                        Id = Guid.Parse("66ba83e9-f3a7-4733-9a40-9a62ddbcc12e"),
                        NotificationType = NotificationType.WeeklyNotification,
                        UserPreference = new WeeklyNotificationUserPreference
                        {
                            Weekday = [Weekday.Friday],
                            NotificationTime = new TimeOnly(18, 0),
                            WorkedHours = 20
                        },
                        IsDisabled = true
                    },
                ]
            }
        };
}