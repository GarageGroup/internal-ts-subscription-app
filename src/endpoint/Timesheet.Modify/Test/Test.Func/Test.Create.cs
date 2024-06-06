using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Modify.Test;

partial class TimesheetModifyFuncTest 
{
    [Fact]
    public static async Task CreateInvokeAsync_InputProjectTypeIsInvalid_ExpectUnknownFailureCode()
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new TimesheetModifyFunc(mockDataverseApi.Object);

        var input = new TimesheetCreateIn(
            systemUserId: new("00889262-2cd5-4084-817b-d810626f2600"),
            date: new(2021, 11, 07),
            project: new(
                id: new("f7410932-b1ee-47b5-844f-7da94836c433"),
                type: (ProjectType)1,
                displayName: "Some name"),
            duration: 3,
            description: "Some text");

        var actual = await func.InvokeAsync(input, default);
        var expected = Failure.Create(TimesheetCreateFailureCode.Unknown, "An unexpected project type: 1");

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task CreateInvokeAsync_InputIsValid_ExpectDataverseImpersonateCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new TimesheetModifyFunc(mockDataverseApi.Object);

        var input = new TimesheetCreateIn(
            systemUserId: new("4698fc58-770b-4a53-adcd-592eaded6f87"),
            date: new(2023, 05, 21),
            project: new(
                id: new("5cef9828-c94b-4ca0-bab5-28c1a45d95ef"),
                type: ProjectType.Lead,
                displayName: default),
            duration: 3,
            description: "Some description text");

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(static a => a.Impersonate(new("4698fc58-770b-4a53-adcd-592eaded6f87")), Times.Once);
    }

    [Theory]
    [MemberData(nameof(TimesheetModifyFuncSource.InputCreateTestData), MemberType = typeof(TimesheetModifyFuncSource))]
    internal static async Task CreateInvokeAsync_InputIsValid_ExpectDataverseCreateCalledOnce(
        TimesheetCreateIn input, DataverseEntityCreateIn<TimesheetJson> expectedInput)
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new TimesheetModifyFunc(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(a => a.CreateEntityAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetCreateFailureCode.Forbidden)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetCreateFailureCode.Forbidden)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, TimesheetCreateFailureCode.Unknown)]
    public static async Task CreateInvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, TimesheetCreateFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);

        var func = new TimesheetModifyFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeTimesheetCreateInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task CreateInvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var func = new TimesheetModifyFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeTimesheetCreateInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}