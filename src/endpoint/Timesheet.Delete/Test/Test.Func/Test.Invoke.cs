using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Delete.Test;

partial class TimesheetDeleteFuncTest
{
    [Theory]
    [MemberData(nameof(TimesheetDeleteFuncSource.InputTestData), MemberType = typeof(TimesheetDeleteFuncSource))]
    public static async Task InvokeAsync_ExpectDataverseDeleteCalledOnce(
        TimesheetDeleteIn input, DataverseEntityDeleteIn expectedInput)
    {
        var mockDataverseApi = BuildMockDataverseDeleteApi(Result.Success<Unit>(default));
        var func = new TimesheetDeleteFunc(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(a => a.DeleteEntityAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task DeleteTimesheetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseDeleteApi(dataverseFailure);
        var func = new TimesheetDeleteFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(Unit.Value, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task DeleteTimesheetAsync_DataverseResultIsFailureRecordNotFound_ExpectSuccess()
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(DataverseFailureCode.RecordNotFound, "Some failure message");

        var mockDataverseApi = BuildMockDataverseDeleteApi(dataverseFailure);
        var func = new TimesheetDeleteFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task DeleteAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApi = BuildMockDataverseDeleteApi(Result.Success<Unit>(default));
        var func = new TimesheetDeleteFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}