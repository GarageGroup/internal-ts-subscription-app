using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test.Test.Func;

partial class NotificationSubscribeFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectSearchForUserOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        await func.InvokeAsync(SomeSubscribeInput, CancellationToken.None);
        
        dataverseApiMock.Verify(
            x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }   
    
    [Fact]
    public static async Task InvokeAsync_ExpectFindUserOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        await func.InvokeAsync(SomeSubscribeInput, CancellationToken.None);
        
        dataverseApiMock.Verify(
            x => x.GetEntityAsync<TelegramBotUserJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }   

    [Fact]
    public static async Task InvokeAsync_ExpectFindNotificationOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        await func.InvokeAsync(SomeSubscribeInput, CancellationToken.None);
        
        dataverseApiMock.Verify(
            x => x.GetEntityAsync<NotificationTypeJson>(It.IsAny<DataverseEntityGetIn>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }   
    
    [Fact]
    public static async Task InvokeAsync_ExpectUpsertNotificationSubscriptionOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        await func.InvokeAsync(SomeSubscribeInput, CancellationToken.None);
        
        dataverseApiMock.Verify(
            x => x.UpdateEntityAsync(It.IsAny<DataverseEntityUpdateIn<NotificationSubscriptionJson>>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }   
    
    [Theory]
    [InlineData(DataverseFailureCode.Unknown, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, NotificationSubscribeFailureCode.BotUserNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, NotificationSubscribeFailureCode.Unknown)]
    public static async Task InvokeAsync_FindBotUserResultIsFailure_ExpectFailure(DataverseFailureCode sourceFailureCode, NotificationSubscribeFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var dataverseApiMock = BuildMockDataverseApiToTestFindBotUser(dataverseFailure);
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        var actual = await func.InvokeAsync(SomeSubscribeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }
    
    [Theory]
    [InlineData(DataverseFailureCode.Unknown, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, NotificationSubscribeFailureCode.NotificationTypeNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidPayload, NotificationSubscribeFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.InvalidFileSize, NotificationSubscribeFailureCode.Unknown)]
    public static async Task InvokeAsync_FindNotificationTypeResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, NotificationSubscribeFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var dataverseApiMock = BuildMockDataverseApiToTestFindNotificationType(dataverseFailure);
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        var actual = await func.InvokeAsync(SomeSubscribeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    public static async Task InvokeAsync_TestValidationOfDailyUserPreferences_ExpectValidationFailure(int dailyWorkedHours)
    {
        var dailyUserPreferences = new DailyNotificationUserPreference
        {
            FlowRuntime = new TimeOnly(0, 0, 0),
            WorkedHours = dailyWorkedHours
        };
        
        var subscriptionInput = new NotificationSubscribeIn(
            botId: long.MaxValue,
            chatId: long.MaxValue, 
            subscriptionData: new DailyNotificationSubscriptionData(dailyUserPreferences)
        );
        
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        var result = await func.InvokeAsync(subscriptionInput, CancellationToken.None);
        
        Assert.True(result.IsFailure);
        Assert.Equal(NotificationSubscribeFailureCode.InvalidQuery, result.FailureOrThrow().FailureCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(40)]
    [InlineData(int.MaxValue)]
    public static async Task InvokeAsync_TestValidationOfDailyUserPreferences_ExpectValidationPass(int dailyWorkedHours)
    {
        var dailyUserPreferences = new DailyNotificationUserPreference
        {
            FlowRuntime = new TimeOnly(0, 0, 0),
            WorkedHours = dailyWorkedHours
        };
        
        var subscriptionInput = new NotificationSubscribeIn(
            botId: long.MaxValue,
            chatId: long.MaxValue, 
            subscriptionData: new DailyNotificationSubscriptionData(dailyUserPreferences)
        );
        
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        var result = await func.InvokeAsync(subscriptionInput, CancellationToken.None);
        
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0, Weekday.Friday)]
    [InlineData(-10, Weekday.Friday)]
    [InlineData(int.MinValue, Weekday.Friday)]
    [InlineData(40)]
    public static async Task InvokeAsync_TestValidationOfWeeklyUserPreferences_ExpectValidationFailure(int weeklyWorkedHours, params Weekday[] weekdays)
    {
        var weeklyUserPreference = new WeeklyNotificationUserPreference
        {
            Weekday = weekdays,
            FlowRuntime = new TimeOnly(0, 0, 0),
            WorkedHours = weeklyWorkedHours
        };
        
        var subscriptionInput = new NotificationSubscribeIn(
            botId: long.MaxValue,
            chatId: long.MaxValue, 
            subscriptionData: new WeeklyNotificationSubscriptionData(weeklyUserPreference)
        );
        
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        var result = await func.InvokeAsync(subscriptionInput, CancellationToken.None);
        
        Assert.True(result.IsFailure);
        Assert.Equal(NotificationSubscribeFailureCode.InvalidQuery, result.FailureOrThrow().FailureCode);
    }
    
    [Theory]
    [InlineData(1, Weekday.Friday)]
    [InlineData(20, Weekday.Friday)]
    [InlineData(int.MaxValue, Weekday.Friday)]
    [InlineData(40, Weekday.Friday, Weekday.Saturday, Weekday.Sunday)]
    [InlineData(40, Weekday.Sunday, Weekday.Saturday)]
    public static async Task InvokeAsync_TestValidationOfWeeklyUserPreferences_ExpectValidationPass(int weeklyWorkedHours, params Weekday[] weekdays)
    {
        var weeklyUserPreference = new WeeklyNotificationUserPreference
        {
            Weekday = weekdays,
            FlowRuntime = new TimeOnly(0, 0, 0),
            WorkedHours = weeklyWorkedHours
        };
        
        var subscriptionInput = new NotificationSubscribeIn(
            botId: long.MaxValue,
            chatId: long.MaxValue, 
            subscriptionData: new WeeklyNotificationSubscriptionData(weeklyUserPreference)
        );
        
        var dataverseApiMock = BuildMockDataverseApi();
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object);

        var result = await func.InvokeAsync(subscriptionInput, CancellationToken.None);
        
        Assert.True(result.IsSuccess);
    }
}