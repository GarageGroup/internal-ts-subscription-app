using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test;

partial class NotificationSubscribeFuncSource
{
    public static TheoryData<NotificationSubscribeIn, NotificationSubscriptionJson> InputValidTestData
        =>
        new()
        {
            {
                new(
                    systemUserId: Guid.Parse("e841030b-abc6-441b-9605-d606da13ca5f"),
                    subscriptionData: new DailyNotificationSubscriptionData(
                        userPreference: default)),
                new()
                {
                    IsDisabled = false
                }
            },
            {
                new(
                    systemUserId: Guid.Parse("5dfd3ac2-7bcb-47dc-b2da-581e6039b692"),
                    subscriptionData: new DailyNotificationSubscriptionData(
                        userPreference: new()
                        {
                            FlowRuntime = new(20, 0, 0),
                            WorkedHours = 8
                        })),
                new()
                {
                    IsDisabled = false,
                    UserPreferences = "{\"workedHours\":8,\"flowRuntime\":\"20:00:00\"}"
                }
            },
            {
                new(
                    systemUserId: Guid.Parse("e2518da8-0f66-464f-b6cd-d22a9c476e6d"),
                    subscriptionData: new WeeklyNotificationSubscriptionData(
                        userPreference: default)),
                new()
                {
                    IsDisabled = false
                }
            },
            {
                new(
                    systemUserId: Guid.Parse("f302bf8b-22a3-4354-bfb9-35a80c61fcdc"),
                    subscriptionData: new WeeklyNotificationSubscriptionData(
                        userPreference: new()
                        {
                            Weekday = [Weekday.Friday],
                            FlowRuntime = new(19, 0, 0),
                            WorkedHours = 35
                        })),
                new()
                {
                    IsDisabled = false,
                    UserPreferences = "{\"weekday\":\"5\",\"workedHours\":35,\"flowRuntime\":\"19:00:00\"}"
                }
            },
            {
                new(
                    systemUserId: Guid.Parse("1d8f2faa-1aa4-4b6e-958f-a455343d9e72"),
                    subscriptionData: new WeeklyNotificationSubscriptionData(
                        userPreference: new()
                        {
                            Weekday = [Weekday.Saturday, Weekday.Friday, Weekday.Sunday],
                            FlowRuntime = new(21, 0, 0),
                            WorkedHours = 40
                        })),
                new()
                {
                    IsDisabled = false,
                    UserPreferences = "{\"weekday\":\"6,5,0\",\"workedHours\":40,\"flowRuntime\":\"21:00:00\"}"
                }
            }
        };
}