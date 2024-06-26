using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using GarageGroup.Internal.Timesheet.Inner.Json;
using GarageGroup.Internal.Timesheet.Option;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Endpoint.Subscription.GetSet.Test;

partial class SubscriptionSetGetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectSearchForUserCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi(SomeTelegramUser, SomeSubscriptions);

        var option = new SubscriptionSetGetOption
        {
            BotId = 10
        };

        var input = new SubscriptionSetGetIn(Guid.Parse("c592bcb6-637b-492c-b923-e2b94b86c71f"));
        var cancellationToken = CancellationToken.None;
        
        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, option);
        _ = await func.InvokeAsync(input, cancellationToken);

        var expected = new DataverseEntityGetIn(
            entityPluralName: "gg_telegram_bot_users",
            selectFields: ["gg_telegram_bot_userid"],
            entityKey: new DataverseAlternateKey(
            [
                new("_gg_systemuser_id_value", "c592bcb6-637b-492c-b923-e2b94b86c71f"),
                new("gg_bot_id", "'10'"),
            ]));
        
        mockDataverseApi.Verify(x => x.GetEntityAsync<TelegramBotUserJson>(expected, cancellationToken), Times.Once);
    }
    
    [Theory]
    [InlineData(DataverseFailureCode.Unknown, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, SubscriptionSetGetFailureCode.BotUserNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, SubscriptionSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, SubscriptionSetGetFailureCode.Unknown)]
    public static async Task InvokeAsync_FindBotUserResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, SubscriptionSetGetFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure, SomeSubscriptions);
        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_ExpectSearchForSubscriptionsCalledOnce()
    {
        var botUser = new TelegramBotUserJson
        {
            Id = Guid.Parse("5a0aa34a-b64a-4258-bed3-5e62946db8b4")
        };
        
        var mockDataverseApi = BuildMockDataverseApi(
            new DataverseEntityGetOut<TelegramBotUserJson>(botUser), 
            SomeSubscriptions);

        var cancellationToken = CancellationToken.None;
        
        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);
        _ = await func.InvokeAsync(SomeInput, cancellationToken);

        var expected = new DataverseEntitySetGetIn(
            entityPluralName: "gg_bot_user_subscriptions",
            selectFields: ["gg_bot_user_subscriptionid", "gg_notification_preferences"],
            expandFields: [ new DataverseExpandedField("gg_notification_type_id", ["gg_key"])],
            filter: $"_gg_bot_user_id_value eq '5a0aa34a-b64a-4258-bed3-5e62946db8b4' and gg_is_disabled eq false");
        
        mockDataverseApi.Verify(x => x.GetEntitySetAsync<SubscriptionJson>(expected, It.IsAny<CancellationToken>()), Times.Once);
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
    public static async Task InvokeAsync_FindSubscriptionsIsFailure_ExpectFailure(DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi(SomeTelegramUser, dataverseFailure);
        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, default);
        var expected = Failure.Create(SubscriptionSetGetFailureCode.Unknown, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(SubscriptionSetGetFuncSource.OutputData), MemberType = typeof(SubscriptionSetGetFuncSource))]
    internal static async Task InvokeAsync_AllResultsAreSuccesses_ExpectSuccess(FlatArray<SubscriptionJson> jsonSubscriptions, FlatArray<Timesheet.Subscription> outputSubscriptions)
    {
        var mockDataverseApi = BuildMockDataverseApi(
            SomeTelegramUser, 
            new DataverseEntitySetGetOut<SubscriptionJson>(jsonSubscriptions));
        var func = new SubscriptionSetGetFunc(mockDataverseApi.Object, SomeOption);

        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);
        var expected = new SubscriptionSetGetOut
        {
            Subscriptions = outputSubscriptions
        };
        
        Assert.StrictEqual(expected, actual);
    }
}