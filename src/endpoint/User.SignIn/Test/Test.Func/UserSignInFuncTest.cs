using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Endpoint.User.SignIn.Test;

public static partial class UserSignInFuncTest
{
    private static readonly UserSignInOption SomeOption
        =
        new()
        {
            BotId = 123123,
            BotName = "Some bot name"
        };

    private static readonly UserSignInIn SomeInput
        =
        new(
            systemUserId: new("c22c378d-7913-4316-8e61-5a5c35987355"),
            chatId: 12312223123);

    private static Mock<IDataverseEntityCreateSupplier> BuildMockDataverseApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityCreateSupplier>();

        _ = mock
            .Setup(static a => a.CreateEntityAsync(It.IsAny<DataverseEntityCreateIn<UserJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}