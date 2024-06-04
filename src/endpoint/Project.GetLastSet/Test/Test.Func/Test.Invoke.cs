using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Project.LastSetGet.Test;

partial class LastProjectSetGetFuncTest
{
    [Theory]
    [MemberData(nameof(LastProjectSetGetFuncSource.InputGetLastTestData), MemberType = typeof(LastProjectSetGetFuncSource))]
    public static async Task InvokeAsync_ExpectMockSqlApiCalledOnce(
        LastProjectSetGetIn input, DbSelectQuery expectedQuery)
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbLastProjectOutput);
        var func = new LastProjectSetGetFunc(mockSqlApi.Object);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbLastProject>(expectedQuery, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_DbResultIsFailure_ExpectUnknownFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some Failure message");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var func = new LastProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(SomeGetLastInput, default);
        var expected = Failure.Create(LastProjectSetGetFailureCode.Unknown, "Some Failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(LastProjectSetGetFuncSource.OutputGetLastTestData), MemberType = typeof(LastProjectSetGetFuncSource))]
    internal static async Task InvokeAsync_DbResultIsSuccess_ExpectSuccess(
        FlatArray<DbLastProject> dbTimesheetProjects, LastProjectSetGetOut expected)
    {
        var mockSqlApi = BuildMockSqlApi(dbTimesheetProjects);
        var func = new LastProjectSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(SomeGetLastInput, default);
        Assert.StrictEqual(expected, actual);
    }
}