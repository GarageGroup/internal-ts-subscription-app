using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Profile.Get.Test;

partial class ProfileGetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectMockSqlApiCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbOutput);

        const long botId = 123123;
        var option = new ProfileGetOption()
        {
            BotId = botId
        };

        var func = new ProfileGetFunc(mockSqlApi.Object, option);

        var cancellationToken = new CancellationToken(false);
        var input = new ProfileGetIn(new("bef33be0-99f5-4018-ba80-3366ec9ec1fd"));

        _ = await func.InvokeAsync(input, cancellationToken);

        var expectedQuery = new DbSelectQuery("gg_telegram_bot_user", "p")
        {
            Top = 1,
            SelectedFields =
            [
                "p.gg_systemuser_idname AS UserName",
                "p.gg_language_code AS LanguageCode"
            ],
            Filter = new DbCombinedFilter(DbLogicalOperator.And)
            {
                Filters =
                [
                    new DbParameterFilter(
                        fieldName: "p.gg_systemuser_id",
                        @operator: DbFilterOperator.Equal,
                        fieldValue: Guid.Parse("bef33be0-99f5-4018-ba80-3366ec9ec1fd"),
                        parameterName: "systemUserId"),
                    new DbParameterFilter(
                        fieldName: "p.gg_bot_id",
                        @operator: DbFilterOperator.Equal,
                        fieldValue: botId,
                        parameterName: "botId")
                ]
            }
        };

        mockSqlApi.Verify(a => a.QueryEntityOrFailureAsync<DbProfile>(expectedQuery, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(EntityQueryFailureCode.Unknown, ProfileGetFailureCode.Unknown)]
    [InlineData(EntityQueryFailureCode.NotFound, ProfileGetFailureCode.NotFound)]
    public static async Task InvokeAsync_DbResultIsFailure_ExpectFailure(
        EntityQueryFailureCode sourceFailureCode, ProfileGetFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var func = new ProfileGetFunc(mockSqlApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    internal static async Task InvokeAsync_DbResultIsSuccess_ExpectSuccess()
    {
        var dbOut = new DbProfile
        {
            UserName = "test",
            LanguageCode = "ru"
        };

        var mockSqlApi = BuildMockSqlApi(dbOut);
        var func = new ProfileGetFunc(mockSqlApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = new ProfileGetOut(
            userName: "test", 
            languageCode: "ru");

        Assert.StrictEqual(expected, actual);
    }
}
