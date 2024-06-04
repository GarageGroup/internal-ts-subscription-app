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
        LastProjectSetGetIn input, LastProjectSetGetOption option, DateOnly today, DbSelectQuery expectedQuery)
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbLastProjectOutput);
        var mockTodayProvider = BuildTodayProvider(today);

        var func = new LastProjectSetGetFunc(mockSqlApi.Object, mockTodayProvider, option);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbLastProject>(expectedQuery, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_DbResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some Failure message");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var mockTodayProvider = BuildTodayProvider(SomeToday);

        var func = new LastProjectSetGetFunc(mockSqlApi.Object, mockTodayProvider, SomeOption);

        var actual = await func.InvokeAsync(SomeGetLastInput, default);
        var expected = Failure.Create(Unit.Value ,"Some Failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(LastProjectSetGetFuncSource.OutputGetLastTestData), MemberType = typeof(LastProjectSetGetFuncSource))]
    internal static async Task InvokeAsync_DbResultIsSuccess_ExpectSuccess(
        FlatArray<DbLastProject> dbTimesheetProjects, LastProjectSetGetOut expected)
    {
        var mockSqlApi = BuildMockSqlApi(dbTimesheetProjects);
        var mockTodayProvider = BuildTodayProvider(SomeToday);
        var func = new LastProjectSetGetFunc(mockSqlApi.Object, mockTodayProvider, SomeOption);

        var actual = await func.InvokeAsync(SomeGetLastInput, default);
        Assert.StrictEqual(expected, actual);
    }
}