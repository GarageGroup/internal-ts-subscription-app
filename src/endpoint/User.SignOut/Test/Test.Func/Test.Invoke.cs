using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Endpoint.User.SignOut.Test;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.User.SignOut.Test;

partial class UserSignOutFuncTest
{
    [Theory]
    [MemberData(nameof(UserSignOutFuncSource.InputTestData), MemberType = typeof(UserSignOutFuncSource))]
    internal static async Task InvokeAsync_ExpectDataverseUpdateCalledOnce(
        UserSignOutOption option, UserSignOutIn input, DataverseEntityUpdateIn<UserJson> expectedInput)
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new UserSignOutFunc(mockDataverseApi.Object, option);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(a => a.UpdateEntityAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var func = new UserSignOutFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create("Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DataverseResultIsRecordNotFoundFailure_ExpectSuccess()
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(DataverseFailureCode.RecordNotFound, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var func = new UserSignOutFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new UserSignOutFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}
