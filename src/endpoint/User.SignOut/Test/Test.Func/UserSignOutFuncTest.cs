using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Endpoint.User.SignOut.Test;

public static partial class UserSignOutFuncTest
{
    private static readonly UserSignOutIn SomeInput
        =
        new(
            systemUserId: new("c22c378d-7913-4316-8e61-5a5c35987355"));

    private static readonly BotInfoGetOut SomeBotInfo
        =
        new(912874398, "SomeBot");

    private static Mock<IDataverseEntityUpdateSupplier> BuildMockDataverseApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityUpdateSupplier>();

        _ = mock.Setup(
            static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<UserJson>>(), It.IsAny<CancellationToken>()))
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