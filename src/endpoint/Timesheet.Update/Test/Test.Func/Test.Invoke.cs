using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Update.Test;

partial class TimesheetUpdateFuncTest
{
    [Fact]
    public static async Task InvokeAsync_InputProjectTypeIsInvalid_ExpectUnknownFailureCode()
    {
        var mockDataverseApi = BuildMockDataverseUpdateApi(Result.Success<Unit>(default));
        var func = new TimesheetUpdateFunc(mockDataverseApi.Object);

        var input = new TimesheetUpdateIn(
            timesheetId: new("b7aee261-3eb4-4d20-8af5-a42c0529a30f"),
            date: new(2021, 11, 07),
            project: new(
                id: new("f7410932-b1ee-47b5-844f-7da94836c433"),
                type: (ProjectType)7,
                displayName: "Some name"),
            duration: 3,
            description: "Some text");

        var actual = await func.InvokeAsync(input, default);
        var expected = Failure.Create(TimesheetUpdateFailureCode.Unknown, "An unexpected project type: 7");

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TimesheetUpdateFuncSource.InputTestData), MemberType = typeof(TimesheetUpdateFuncSource))]
    internal static async Task InvokeAsync_InputIsNotNull_ExpectDataverseUpdateCalledOnce(
        TimesheetUpdateIn input, DataverseEntityUpdateIn<TimesheetJson> expectedInput)
    {
        var mockDataverseApi = BuildMockDataverseUpdateApi(Result.Success<Unit>(default));
        var func = new TimesheetUpdateFunc(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(a => a.UpdateEntityAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetUpdateFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetUpdateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, TimesheetUpdateFailureCode.Unknown)]
    public static async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, TimesheetUpdateFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseUpdateApi(dataverseFailure);
        var func = new TimesheetUpdateFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApi = BuildMockDataverseUpdateApi(Result.Success<Unit>(default));
        var func = new TimesheetUpdateFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}
