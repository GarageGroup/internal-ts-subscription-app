using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.User.SignIn.Test;

partial class UserSignInFuncTest
{
    [Fact]
    internal static async Task InvokeAsync_ExpectDataverseGetCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi(SomeSystemUserResult, Result.Success<Unit>(default));
        var func = new UserSignInFunc(mockDataverseApi.Object, SomeOption);

        var cancellationToken = new CancellationToken(false);

        var input = new UserSignInIn(
            systemUserId: new("3f414904-3128-4f27-af56-d5f45bf31dd5"),
            chatId: 123);
        _ = await func.InvokeAsync(input, cancellationToken);

        var expectedInput = new DataverseEntityGetIn(
            entityPluralName: "systemusers",
            entityKey: new DataversePrimaryKey(new("3f414904-3128-4f27-af56-d5f45bf31dd5")),
            selectFields: ["yomifullname"]);
        mockDataverseApi.Verify(a => a.GetEntityAsync<SystemUserJson>(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, UserSignInFailureCode.SystemUserNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, UserSignInFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, UserSignInFailureCode.Unknown)]
    public static async Task InvokeAsync_DataverseGetResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, UserSignInFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure, Result.Success<Unit>(default));
        var func = new UserSignInFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(UserSignInFuncSource.InputTestData), MemberType = typeof(UserSignInFuncSource))]
    internal static async Task InvokeAsync_ExpectUpdateDataverseUpdateCalledOnce(
        UserSignInOption option, UserSignInIn input, DataverseEntityGetOut<SystemUserJson> systemUserResult, DataverseEntityUpdateIn<UserJson> expectedInput)
    {
        var mockDataverseApi = BuildMockDataverseApi(systemUserResult, Result.Success<Unit>(default));
        var func = new UserSignInFunc(mockDataverseApi.Object, option);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(a => a.UpdateEntityAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.RecordNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task InvokeAsync_DataverseUpdateResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(SomeSystemUserResult, dataverseFailure);
        var func = new UserSignInFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(UserSignInFailureCode.Unknown, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApi = BuildMockDataverseApi(SomeSystemUserResult, Result.Success<Unit>(default));
        var func = new UserSignInFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}
