using GarageGroup.Infra;
using System;
using System.Text.RegularExpressions;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class UserSignInFunc(IDataverseApiClient dataverseApi, UserSignInOption option) : IUserSignInFunc
{
    private const string DefaultLanguageCode = "en";

    private const string TelegramWebAppData = "WebAppData";

    private const string HashParameterName = "hash=";

    [GeneratedRegex(@"""id"":(\d+)")]
    private static partial Regex CreateUserIdRegex();

    private static readonly Regex UserIdRegex;

    static UserSignInFunc()
        =>
        UserIdRegex = CreateUserIdRegex();

    private static UserSignInFailureCode ToUserSignInFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => UserSignInFailureCode.SystemUserNotFound,
            _ => default
        };

    private sealed record class UserChatId
    {
        public Guid SystemUserId { get; init; }

        public long ChatId { get; init; }
    }
}