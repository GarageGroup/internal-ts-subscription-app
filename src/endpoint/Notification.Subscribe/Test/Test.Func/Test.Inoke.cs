using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Endpoint.Notification.Subscribe.Test.Test.Func;

partial class NotificationSubscribeFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectSearchForUserOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult:new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var notificationSubscribeOption = new NotificationSubscribeOption
        {
            BotId = 10
        };

        var input = new NotificationSubscribeIn(
            Guid.Parse("8235b18a-04e8-4092-985b-c280b5810ff0"), 
            new DailyNotificationSubscriptionData(null));
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, notificationSubscribeOption);
        await func.InvokeAsync(input, default);

        var expected = new DataverseEntityGetIn(
            entityPluralName: "gg_telegram_bot_users",
            selectFields: ["gg_telegram_bot_userid"],
            entityKey: new DataverseAlternateKey(
            [
                new("_gg_systemuser_id_value", "8235b18a-04e8-4092-985b-c280b5810ff0"),
                new("gg_bot_id", "'10'"),
            ]));
        
        dataverseApiMock.Verify(
            x => x.GetEntityAsync<TelegramBotUserJson>(expected, It.IsAny<CancellationToken>()), 
            Times.Once);
    }   

    [Fact]
    public static async Task InvokeAsync_ExpectFindDailyNotificationTypeOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult: new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var input = new NotificationSubscribeIn(
            Guid.Parse("8235b18a-04e8-4092-985b-c280b5810ff0"), 
            new DailyNotificationSubscriptionData(null));
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
        await func.InvokeAsync(input, default);

        var expected = new DataverseEntityGetIn(
            entityPluralName: "gg_bot_notification_types",
            selectFields: ["gg_bot_notification_typeid"],
            entityKey: new DataverseAlternateKey("gg_key", "'dailyTimesheetNotification'"));
        
        dataverseApiMock.Verify(
            x => x.GetEntityAsync<NotificationTypeJson>(expected, It.IsAny<CancellationToken>()), 
            Times.Once);
    }   
    
    [Fact]
    public static async Task InvokeAsync_ExpectFindWeeklyNotificationTypeOnce()
    {
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult: new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
 
        var input = new NotificationSubscribeIn(
            Guid.Parse("8235b18a-04e8-4092-985b-c280b5810ff0"), 
            new WeeklyNotificationSubscriptionData(null));
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
        await func.InvokeAsync(input, default);

        var expected = new DataverseEntityGetIn(
            entityPluralName: "gg_bot_notification_types",
            selectFields: ["gg_bot_notification_typeid"],
            entityKey: new DataverseAlternateKey("gg_key", "'weeklyTimesheetNotification'"));
        
        dataverseApiMock.Verify(
            x => x.GetEntityAsync<NotificationTypeJson>(expected, It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public static async Task InvokeAsync_ExpectUpsertNotificationSubscriptionOnce()
    {
        var botUser = new TelegramBotUserJson
        {
            Id = Guid.Parse("1c89a21f-533f-41db-9855-9839e5685bad")
        };

        var notificationType = new NotificationTypeJson()
        {
            Id = Guid.Parse("9e7abfe2-12be-435b-bc40-795ce1a4212f")
        };
        
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(botUser),
            notificationTypeGetResult: new DataverseEntityGetOut<NotificationTypeJson>(notificationType),
            subscriptionUpdateResult: Unit.Value);
        
        var input = new NotificationSubscribeIn(
            Guid.Parse("8235b18a-04e8-4092-985b-c280b5810ff0"), 
            new DailyNotificationSubscriptionData(null));
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
        await func.InvokeAsync(input, default);

        var expected = new DataverseEntityUpdateIn<NotificationSubscriptionJson>(
            entityPluralName: "gg_bot_user_subscriptions",
            entityKey: new DataverseAlternateKey(
                [
                    new ("_gg_bot_user_id_value", "1c89a21f-533f-41db-9855-9839e5685bad"),
                    new ("_gg_notification_type_id_value", "9e7abfe2-12be-435b-bc40-795ce1a4212f")
                ]),
            entityData: new NotificationSubscriptionJson { IsDisabled = false })
        {
            OperationType = DataverseUpdateOperationType.Upsert
        };
            
        dataverseApiMock.Verify(
            x => x.UpdateEntityAsync(expected, It.IsAny<CancellationToken>()), 
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
    
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: dataverseFailure,
            notificationTypeGetResult: new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
    
        var actual = await func.InvokeAsync(SomeSubscribeIn, default);
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
    
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult: dataverseFailure,
            subscriptionUpdateResult: Unit.Value);
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
    
        var actual = await func.InvokeAsync(SomeSubscribeIn, default);
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
            systemUserId: Guid.Parse("152fea7a-61e3-4f45-8715-a5591e839126"),
            subscriptionData: new DailyNotificationSubscriptionData(dailyUserPreferences)
        );
        
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult:new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
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
            systemUserId: Guid.Parse("152fea7a-61e3-4f45-8715-a5591e839126"),
            subscriptionData: new DailyNotificationSubscriptionData(dailyUserPreferences)
        );
        
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult:new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
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
            systemUserId: Guid.Parse("152fea7a-61e3-4f45-8715-a5591e839126"),
            subscriptionData: new WeeklyNotificationSubscriptionData(weeklyUserPreference)
        );
        
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult:new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
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
            systemUserId: Guid.Parse("152fea7a-61e3-4f45-8715-a5591e839126"),
            subscriptionData: new WeeklyNotificationSubscriptionData(weeklyUserPreference)
        );
        
        var dataverseApiMock = BuildMockDataverseApi(
            botUserGetResult: new DataverseEntityGetOut<TelegramBotUserJson>(new TelegramBotUserJson()),
            notificationTypeGetResult:new DataverseEntityGetOut<NotificationTypeJson>(new NotificationTypeJson()),
            subscriptionUpdateResult: Unit.Value);
        
        var func = new NotificationSubscribeFunc(dataverseApiMock.Object, SomeNotificationSubscribeOption);
        var result = await func.InvokeAsync(subscriptionInput, CancellationToken.None);
        
        Assert.True(result.IsSuccess);
    }
}