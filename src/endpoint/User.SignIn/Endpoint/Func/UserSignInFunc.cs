using GarageGroup.Infra;
using System;
using System.Text.RegularExpressions;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class UserSignInFunc(IDataverseApiClient dataverseApi, UserSignInOption option) : IUserSignInFunc
{
    private const string DefaultLanguageCode = "en";

    private const string RegexPattern = @"""id"":(\d+)";

    private const string TelegramWebAppData = "WebAppData";

    private const string HashParameterName = "hash=";

    [GeneratedRegex(RegexPattern)]
    private static partial Regex Regex();

    private sealed record class UserChatId
    {
        public Guid SystemUserId { get; init; }

        public long ChatId { get; init; }
    }
}