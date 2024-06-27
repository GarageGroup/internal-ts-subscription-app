using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

partial class SubscriptionSetGetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectDataverseGetSetCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi(SomeDataverseSubscriptionsOut);

        var option = new SubscriptionSetGetOption
        {
            BotId = 5674427344
        };

        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, option);

        var input = new SubscriptionSetGetIn(
            systemUserId: new("5a0aa34a-b64a-4258-bed3-5e62946db8b4"));

        var cancellationToken = new CancellationToken(canceled: false);

        _ = await func.InvokeAsync(input, cancellationToken);

        var expectedInput = new DataverseEntitySetGetIn(
            entityPluralName: "gg_bot_user_subscriptions",
            selectFields: ["gg_notification_preferences"],
            expandFields: [new("gg_notification_type_id", ["gg_key"])],
            filter:
                "(gg_bot_user_id%2f_gg_systemuser_id_value eq '5a0aa34a-b64a-4258-bed3-5e62946db8b4'" +
                " and gg_bot_user_id%2fgg_bot_id eq '5674427344' and gg_is_disabled ne true)");
        
        mockDataverseApi.Verify(x => x.GetEntitySetAsync<SubscriptionJson>(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.RecordNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task InvokeAsync_DataverseGetSetResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(SubscriptionSetGetFailureCode.Unknown, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(SubscriptionSetGetFuncSource.OutputInvalidTestData), MemberType = typeof(SubscriptionSetGetFuncSource))]
    internal static async Task InvokeAsync_DataverseGetSetResultNotificationTypeIsInvalid_ExpectInvalidOperationException(
        FlatArray<SubscriptionJson> jsonSubscriptions, string expectedErrorMessage)
    {
        var dataverseSubscriptionsOut = new DataverseEntitySetGetOut<SubscriptionJson>(jsonSubscriptions);
        var mockDataverseApi = BuildMockDataverseApi(dataverseSubscriptionsOut);

        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(TestAsync);

        Assert.Equal(expectedErrorMessage, ex.Message);

        async Task TestAsync()
            =>
            await func.InvokeAsync(SomeInput, default);
    }

    [Theory]
    [MemberData(nameof(SubscriptionSetGetFuncSource.OutputValidTestData), MemberType = typeof(SubscriptionSetGetFuncSource))]
    internal static async Task InvokeAsync_DataverseGetSetResultIsSuccess_ExpectSuccess(
        FlatArray<SubscriptionJson> jsonSubscriptions, SubscriptionSetGetOut expected)
    {
        var dataverseSubscriptionsOut = new DataverseEntitySetGetOut<SubscriptionJson>(jsonSubscriptions);
        var mockDataverseApi = BuildMockDataverseApi(dataverseSubscriptionsOut);

        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);
        var actual = await func.InvokeAsync(SomeInput, default);

        Assert.StrictEqual(expected, actual);
    }
}