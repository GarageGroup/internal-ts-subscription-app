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

    private static readonly DataverseEntityGetOut<SystemUserJson> SomeSystemUserResult
        =
        new(
            value: new()
            {
                FullName = "Some user name"
            });

    private static Mock<IDataverseApiClient> BuildMockDataverseApi(
        in Result<DataverseEntityGetOut<SystemUserJson>, Failure<DataverseFailureCode>> getResult,
        in Result<Unit, Failure<DataverseFailureCode>> upsertResult)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock
            .Setup(static a => a.GetEntityAsync<SystemUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getResult);

        _ = mock
            .Setup(static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<UserJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(upsertResult);

        return mock;
    }
}