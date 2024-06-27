using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

public static partial class SubscriptionSetGetFuncTest
{
    private static readonly SubscriptionSetGetOption SomeOption
        =
        new()
        {
            BotId = 10
        };

    private static readonly SubscriptionSetGetIn SomeInput
        =
        new(Guid.Parse("f80f4725-c646-4d98-9994-5ee5a392a90b"));

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
        var mockDataverseApi = new Mock<IDataverseEntitySetGetSupplier>();
        
        _ = mockDataverseApi
            .Setup(static x => x.GetEntitySetAsync<SubscriptionJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        
        return mockDataverseApi;
    }
}