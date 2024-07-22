using GarageGroup.Infra;
using System;
using System.Text.RegularExpressions;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class UserSignInFunc : IUserSignInFunc
{
    private const string TelegramWebAppData = "WebAppData";

    private const string HashParameterName = "hash=";

    [GeneratedRegex(@"""id"":(\d+)")]
    private static partial Regex CreateUserIdRegex();

    private static readonly Regex UserIdRegex;

    static UserSignInFunc()
        =>
        UserIdRegex = CreateUserIdRegex();

    private readonly IDataverseApiClient dataverseApi;

    private readonly IBotInfoGetSupplier botApi;

    private readonly UserSignInOption option;

    internal UserSignInFunc(IDataverseApiClient dataverseApi, IBotInfoGetSupplier botApi, UserSignInOption option)
    {
        this.dataverseApi = dataverseApi;
        this.botApi = botApi;
        this.option = option;
    }

    private static UserSignInFailureCode ToUserSignInFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => UserSignInFailureCode.SystemUserNotFound,
            _ => default
        };

    private sealed record class UserChatId
    {
        public required Guid SystemUserId { get; init; }

        public required long ChatId { get; init; }
    }
}