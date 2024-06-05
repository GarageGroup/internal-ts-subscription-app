using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Timesheet.Get.Test;

partial class TimesheetSetGetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectSqlApiCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var func = new TimesheetSetGetFunc(mockSqlApi.Object);

        var input = new TimesheetSetGetIn(
            systemUserId: new("bd8b8e33-554e-e611-80dc-c4346bad0190"),
            date: new(2022, 02, 05));

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        var expectedQuery = new DbSelectQuery("gg_timesheetactivity", "t")
        {
            SelectedFields =
            [
                "t.gg_duration AS Duration",
                "t.regardingobjectid AS ProjectId",
                "t.regardingobjecttypecode AS ProjectTypeCode",
                "t.regardingobjectidname AS ProjectName",
                "t.subject AS Subject",
                "t.gg_description AS Description",
                "t.activityid AS Id",
                "i.statecode AS IncidentStateCode",
                "t.statecode AS TimesheetStateCode"
            ],
            JoinedTables =
            [
                new (DbJoinType.Left, "incident", "i", "t.regardingobjectid = i.incidentid")
            ],
            Filter = new DbCombinedFilter(DbLogicalOperator.And)
            {
                Filters =
                [
                    new DbParameterFilter("t.ownerid", DbFilterOperator.Equal, Guid.Parse("bd8b8e33-554e-e611-80dc-c4346bad0190"), "ownerId"),
                    new DbParameterFilter("t.gg_date", DbFilterOperator.Equal, "2022-02-05", "date"),
                    new DbParameterArrayFilter(
                        fieldName: "t.regardingobjecttypecode",
                        @operator: DbArrayFilterOperator.In,
                        fieldValues: new(3, 4, 112, 10912),
                        parameterPrefix: "projectTypeCode")
                ]
            },
            Orders =
            [
                new("t.createdon", DbOrderType.Ascending)
            ]
        };

        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbTimesheet>(expectedQuery, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_DbResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some failure message");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var func = new TimesheetSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(SomeTimesheetSetGetInput, default);
        var expected = Failure.Create(Unit.Value, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TimesheetSetGetFuncSource.OutputGetTestData), MemberType = typeof(TimesheetSetGetFuncSource))]
    internal static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess(
        FlatArray<DbTimesheet> dbTimesheets, TimesheetSetGetOut expected)
    {
        var mockSqlApi = BuildMockSqlApi(dbTimesheets);
        var func = new TimesheetSetGetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(SomeTimesheetSetGetInput, default);
        Assert.StrictEqual(expected, actual);
    }
}