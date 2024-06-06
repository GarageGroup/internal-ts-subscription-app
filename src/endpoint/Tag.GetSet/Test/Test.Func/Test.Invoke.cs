using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Tag.GetSet.Test;

partial class TagGetSetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectSqlApiCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetTagSet);
        var func = new TagGetSetFunc(mockSqlApi.Object);

        var input = new TagSetGetIn(
            systemUserId: new("82ee3d26-17f1-4e2f-adb2-eeea5119a512"),
            projectId: new("58482d23-ca3e-4499-8294-cc9b588cce73"),
            minDate: new(2023, 06, 15),
            maxDate: new(2023, 11, 03));

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(input, cancellationToken);

        var expectedQuery = new DbSelectQuery("gg_timesheetactivity", "t")
        {
            SelectedFields = new("t.gg_description AS Description"),
            Filter = new DbCombinedFilter(DbLogicalOperator.And)
            {
                Filters =
                [
                    new DbParameterFilter(
                        "t.ownerid", DbFilterOperator.Equal, Guid.Parse("82ee3d26-17f1-4e2f-adb2-eeea5119a512"), "ownerId"),
                    new DbParameterFilter(
                        "t.regardingobjectid", DbFilterOperator.Equal, Guid.Parse("58482d23-ca3e-4499-8294-cc9b588cce73"), "projectId"),
                    new DbLikeFilter(
                        "t.gg_description", "%#%", "description"),
                    new DbParameterFilter(
                        "t.gg_date", DbFilterOperator.GreaterOrEqual, "2023-06-15", "minDate"),
                    new DbParameterFilter(
                        "t.gg_date", DbFilterOperator.LessOrEqual, "2023-11-03", "maxDate")
                ]
            },
            Orders =
            [
                new("t.gg_date", DbOrderType.Descending),
                new("t.createdon", DbOrderType.Descending)
            ]
        };

        mockSqlApi.Verify(a => a.QueryEntitySetOrFailureAsync<DbTag>(expectedQuery, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task InvokeAsync_DbResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some failure text");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var func = new TagGetSetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(SomeTimesheetTagSetGetInput, default);
        var expected = Failure.Create("Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TagGetSetFuncSource.OutputTestData), MemberType = typeof(TagGetSetFuncSource))]
    internal static async Task InvokeAsync_DbResultIsSuccess_ExpectSuccess(
        FlatArray<DbTag> dbTimesheetTags, TagSetGetOut expected)
    {
        var mockSqlApi = BuildMockSqlApi(dbTimesheetTags);
        var func = new TagGetSetFunc(mockSqlApi.Object);

        var actual = await func.InvokeAsync(SomeTimesheetTagSetGetInput, default);
        Assert.StrictEqual(expected, actual);
    }
}