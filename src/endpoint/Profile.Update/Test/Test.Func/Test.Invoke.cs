using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.Update.Test.Test;

partial class ProfileUpdateFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectDataverseUpdateCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));

        var option = new ProfileUpdateOption()
        {
            BotId = 123123
        };
        var func = new ProfileUpdateFunc(mockDataverseApi.Object, option);

        var cancellationToken = new CancellationToken(false);

        var input = new ProfileUpdateIn(
            systemUserId: new("2e159d1a-42ac-4c9f-af70-b094ba32a786"),
            languageCode: "en");
        _ = await func.InvokeAsync(input, cancellationToken);

        var expectedInput = new DataverseEntityUpdateIn<ProfileJson>(
            entityPluralName: "gg_telegram_bot_users",
            entityData: new ProfileJson()
            {
                LanguageCode = "en",
            },
            entityKey: new DataverseAlternateKey(
            [
                new("_gg_systemuser_id_value", "2e159d1a-42ac-4c9f-af70-b094ba32a786"),
                new("gg_bot_id", "'123123'")
            ]));
        mockDataverseApi.Verify(a => a.UpdateEntityAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, ProfileUpdateFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, ProfileUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, ProfileUpdateFailureCode.Unknown)]
    public static async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, ProfileUpdateFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var func = new ProfileUpdateFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new ProfileUpdateFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}