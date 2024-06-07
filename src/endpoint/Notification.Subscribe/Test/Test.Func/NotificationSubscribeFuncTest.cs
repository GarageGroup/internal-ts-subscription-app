using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test.Test.Func;

public static partial class NotificationSubscribeFuncTest
{
    private static readonly NotificationSubscribeOption SomeNotificationSubscribeOption 
        = 
        new ()
        {
            BotId = 10
        };

    private static readonly NotificationSubscribeIn SomeSubscribeIn 
        = 
        new(
            systemUserId: Guid.Parse("ac9bdaf3-a9c0-4b8b-833a-406bf59a9fcd"),
            subscriptionData: new DailyNotificationSubscriptionData(null));
    
    private static Mock<IDataverseApiClient> BuildMockDataverseApi(
        Result<DataverseEntityGetOut<TelegramBotUserJson>, Failure<DataverseFailureCode>> botUserGetResult,
        Result<DataverseEntityGetOut<NotificationTypeJson>, Failure<DataverseFailureCode>> notificationTypeGetResult,
        Result<Unit, Failure<DataverseFailureCode>> subscriptionUpdateResult)
    {
        var mock = new Mock<IDataverseApiClient>();
        
        mock.Setup(x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(botUserGetResult);

        mock.Setup(x => x.GetEntityAsync<NotificationTypeJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(notificationTypeGetResult);

        mock.Setup(x => x.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<NotificationSubscriptionJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscriptionUpdateResult);

        return mock;
    }
}