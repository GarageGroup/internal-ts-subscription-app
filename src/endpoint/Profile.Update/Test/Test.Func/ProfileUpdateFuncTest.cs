using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.Update.Test.Test;

public static partial class ProfileUpdateFuncTest
{
    private static readonly BotInfoGetOut SomeBotInfo
        =
        new(178924712, "some_name");

    private static readonly ProfileUpdateIn SomeInput
        =
        new(
            systemUserId: new("c22c378d-7913-4316-8e61-5a5c35987355"),
            languageCode: ProfileLanguage.English);

    private static Mock<IDataverseEntityUpdateSupplier> BuildMockDataverseApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityUpdateSupplier>();

        _ = mock.Setup(
            static a => a.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<ProfileJson>>(), It.IsAny<CancellationToken>()))
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