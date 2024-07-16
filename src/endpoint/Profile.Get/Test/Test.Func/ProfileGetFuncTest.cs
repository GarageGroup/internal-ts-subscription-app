using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.Get.Test;

public static partial class ProfileGetFuncTest
{
    private static readonly ProfileGetIn SomeInput
        =
        new(
            systemUserId: new("45b6e085-4d6e-4b2d-a26c-eb8c1c5a2e5e"));

    private static readonly DbProfile SomeDbProfile
        =
        new()
        {
            UserName = "Some user name",
            LanguageCode = "en"
        };

    private static readonly BotInfoGetOut SomeBotInfo
        =
        new(918248127, "SomeBot");

    private static Mock<ISqlQueryEntitySupplier> BuildMockSqlApi(
        in Result<DbProfile, Failure<EntityQueryFailureCode>> result)
    {
        var mock = new Mock<ISqlQueryEntitySupplier>();

        _ = mock
            .Setup(static a => a.QueryEntityOrFailureAsync<DbProfile>(It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
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