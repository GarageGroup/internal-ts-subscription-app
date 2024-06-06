using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test.Test.Func;

public static partial class NotificationSubscribeFuncTest
{
    private static readonly NotificationSubscribeIn SomeSubscribeInput
        =  
        new(
            botId: long.MaxValue,
            chatId: long.MaxValue, 
            subscriptionData: new DailyNotificationSubscriptionData(null));
    
    private static Mock<IDataverseApiClient> BuildMockDataverseApi()
    {
        var mock = new Mock<IDataverseApiClient>();
        
        mock.Setup(x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()));

        mock.Setup(x => x.GetEntityAsync<NotificationTypeJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()));

        mock.Setup(x => x.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<NotificationSubscriptionJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        return mock;
    }

    private static Mock<IDataverseApiClient> BuildMockDataverseApiToTestFindBotUser(Failure<DataverseFailureCode> failure)
    {
        var mock = new Mock<IDataverseApiClient>();

        mock.Setup(x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(failure);

        mock.Setup(x => x.GetEntityAsync<NotificationTypeJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()));
        
        return mock;
    }
    
    private static Mock<IDataverseApiClient> BuildMockDataverseApiToTestFindNotificationType(Failure<DataverseFailureCode> failure)
    {
        var mock = new Mock<IDataverseApiClient>();

        mock.Setup(x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()));
        
        mock.Setup(x => x.GetEntityAsync<NotificationTypeJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(failure);

        return mock;
    }
}