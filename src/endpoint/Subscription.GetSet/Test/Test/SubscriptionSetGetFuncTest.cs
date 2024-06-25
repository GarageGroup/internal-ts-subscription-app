using System;
using System.Threading;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Inner.Json;
using GarageGroup.Internal.Timesheet.Option;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

public static partial class SubscriptionSetGetFuncTest
{
    private static readonly DataverseEntityGetOut<TelegramBotUserJson> SomeTelegramUser
        =
        new(new()
        {
            Id = Guid.Parse("c877a7ce-b716-48f6-940d-9aa1e1509198")
        });

    private static readonly DataverseEntitySetGetOut<SubscriptionJson> SomeSubscriptions 
        =
        new([
            new()
            {
                Id = Guid.Parse("da4fbde4-65f4-42aa-be80-b3711f6e80c8"),
                NotificationType = new NotificationTypeJson
                {
                    Key = "dailyTimesheetNotification"
                }
            },
            new()
            {
                Id = Guid.Parse("ebfeea3e-36ce-477a-b2a0-f6cdfd5427e1"),
                NotificationType = new NotificationTypeJson
                {
                    Key = "weeklyTimesheetNotification"
                }
            },
        ]);

    private static readonly SubscriptionSetGetOption SomeOption
        =
        new()
        {
            BotId = 10
        };

    private static readonly SubscriptionSetGetIn SomeInput
        =
        new(Guid.Parse("f80f4725-c646-4d98-9994-5ee5a392a90b"));
    
    private static Mock<IDataverseApiClient> BuildMockDataverseApi(
        in Result<DataverseEntityGetOut<TelegramBotUserJson>, Failure<DataverseFailureCode>> getTelegramBotUserResult,
        in Result<DataverseEntitySetGetOut<SubscriptionJson>, Failure<DataverseFailureCode>> getSubscriptionsResult)
    {
        var mockDataverseApi = new Mock<IDataverseApiClient>();

        mockDataverseApi
            .Setup(x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getTelegramBotUserResult);
        
        mockDataverseApi
            .Setup(x=> x.GetEntitySetAsync<SubscriptionJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getSubscriptionsResult);
        
        return mockDataverseApi;
    }
}