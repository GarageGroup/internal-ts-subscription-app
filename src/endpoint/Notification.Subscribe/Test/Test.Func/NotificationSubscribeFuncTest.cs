using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test.Test.Func;

public static partial class NotificationSubscribeFuncTest
{
    private static readonly NotificationSubscribeOption SomeOption
        = 
        new ()
        {
            BotId = 8912380915
        };

    private static readonly NotificationSubscribeIn SomeSubscribeInput
        = 
        new(
            systemUserId: Guid.Parse("ac9bdaf3-a9c0-4b8b-833a-406bf59a9fcd"),
            subscriptionData: new DailyNotificationSubscriptionData(null));

    private static readonly DataverseEntityGetOut<TelegramBotUserJson> SomeDataverseTelegramBotUser
        =
        new(
            value: new()
            {
                Id = Guid.Parse("f8b5c153-58cd-4afc-ab89-a6dfe77c7216")
            });

    private static readonly DataverseEntityGetOut<NotificationTypeJson> SomeDataverseNotificationType
        =
        new(
            value: new()
            {
                Id = Guid.Parse("fcaa0b1e-1265-4c17-ac9d-ed5e6e8c63f2")
            });

    private static Mock<IDataverseApiClient> BuildMockDataverseApi(
        in Result<DataverseEntityGetOut<TelegramBotUserJson>, Failure<DataverseFailureCode>> botUserGetResult,
        in Result<DataverseEntityGetOut<NotificationTypeJson>, Failure<DataverseFailureCode>> notificationTypeGetResult,
        in Result<Unit, Failure<DataverseFailureCode>> subscriptionUpdateResult)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock.Setup(
            static x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(botUserGetResult);

        _ = mock.Setup(
            static x => x.GetEntityAsync<NotificationTypeJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(notificationTypeGetResult);

        _ = mock.Setup(
            static x => x.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<NotificationSubscriptionJson>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(subscriptionUpdateResult);

        return mock;
    }
}