using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

public static partial class SubscriptionSetGetFuncTest
{
    private static readonly BotInfoGetOut SomeBotInfo
        =
        new(3181349813, "SomeBot");

    private static readonly SubscriptionSetGetIn SomeInput
        =
        new(
            systemUserId: new("f80f4725-c646-4d98-9994-5ee5a392a90b"));

    private static readonly DataverseEntitySetGetOut<SubscriptionJson> SomeDataverseSubscriptionsOut
        =
        new(
            value:
            [
                new()
                {
                    NotificationType = new()
                    {
                        Key = "dailyTimesheetNotification"
                    }
                },
                new()
                {
                    NotificationType = new()
                    {
                        Key = "weeklyTimesheetNotification"
                    }
                },
            ]);
    
    private static Mock<IDataverseEntitySetGetSupplier> BuildMockDataverseApi(
        in Result<DataverseEntitySetGetOut<SubscriptionJson>, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntitySetGetSupplier>();
        
        _ = mock.Setup(
            static x => x.GetEntitySetAsync<SubscriptionJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);
        
        return mock;
    }

    private static Mock<IBotInfoGetSupplier> BuildMockBotApi(
        in Result<BotInfoGetOut, Failure<Unit>> result)
    {
        var mock = new Mock<IBotInfoGetSupplier>();

        _ = mock.Setup(
            static a => a.GetBotInfoAsync(It.IsAny<Unit>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

        return mock;
    }
}