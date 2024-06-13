using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.Update.Test.Test;

public static partial class ProfileUpdateFuncTest
{
    private static readonly ProfileUpdateOption SomeOption
        =
        new()
        {
            BotId = 123123
        };

    private static readonly ProfileUpdateIn SomeInput
        =
        new(
            systemUserId: new("c22c378d-7913-4316-8e61-5a5c35987355"),
            languageCode: ProfileLanguage.English);

    private static Mock<IDataverseEntityUpdateSupplier> BuildMockDataverseApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityUpdateSupplier>();

        _ = mock
            .Setup(static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<ProfileJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}