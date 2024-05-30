using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.SearchSet.Test;

partial class ProjectSetSearchFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectDataverseImpersonateCalledOnce()
    {
        var mockDataverseSearchApi = BuildMockDataverseSearchApi(SomeDataverseOutput);
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseSearchApi.Object);

        var func = new ProjectSetSearchFunc(mockDataverseApi.Object);

        var input = new ProjectSetSearchIn(
            systemUserId: new("3589bc60-227f-4aa6-a5c3-4248304a1b49"),
            searchText: "Some search text",
            top: 3);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseApi.Verify(static a => a.Impersonate(new("3589bc60-227f-4aa6-a5c3-4248304a1b49")), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ProjectSetSearchFuncSource.InputTestData), MemberType = typeof(ProjectSetSearchFuncSource))]
    public static async Task InvokeAsync_ExpectDataverseSearchCalledOnce(
        ProjectSetSearchIn input, DataverseSearchIn expectedInput)
    {
        var mockDataverseSearchApi = BuildMockDataverseSearchApi(SomeDataverseOutput);
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseSearchApi.Object);

        var func = new ProjectSetSearchFunc(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockDataverseSearchApi.Verify(a => a.SearchAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, ProjectSetSearchFailureCode.Forbidden)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, ProjectSetSearchFailureCode.Forbidden)]
    [InlineData(DataverseFailureCode.Throttling, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, ProjectSetSearchFailureCode.Forbidden)]
    [InlineData(DataverseFailureCode.DuplicateRecord, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, ProjectSetSearchFailureCode.Unknown)]
    public static async Task SearchProjectSetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, ProjectSetSearchFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseSearchApi = BuildMockDataverseSearchApi(dataverseFailure);
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseSearchApi.Object);

        var func = new ProjectSetSearchFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(SomeSearchInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProjectSetSearchFuncSource.OutputTestData), MemberType = typeof(ProjectSetSearchFuncSource))]
    public static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess(
        DataverseSearchOut dataverseOutput, ProjectSetSearchOut expected)
    {
        var mockDataverseSearchApi = BuildMockDataverseSearchApi(dataverseOutput);
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseSearchApi.Object);

        var func = new ProjectSetSearchFunc(mockDataverseApi.Object);
        var actual = await func.InvokeAsync(SomeSearchInput, default);

        Assert.StrictEqual(expected, actual);
    }
}