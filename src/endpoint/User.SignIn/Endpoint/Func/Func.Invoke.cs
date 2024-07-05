using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class UserSignInFunc
{
    public ValueTask<Result<Unit, Failure<UserSignInFailureCode>>> InvokeAsync(
        UserSignInIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ParseTelegramDataOrFailure)
        .ForwardValue(
            InnerInvokeAsync);

    private ValueTask<Result<Unit, Failure<UserSignInFailureCode>>> InnerInvokeAsync(
        UserChatId input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.SystemUserId, cancellationToken)
        .Pipe(
            SystemUserJson.BuildDataverseInput)
        .PipeValue(
            dataverseApi.GetEntityAsync<SystemUserJson>)
        .Map(
            user => new UserJson
            {
                BotId = option.BotId,
                BotName = $"{option.BotName} - {user.Value.FullName}",
                ChatId = input.ChatId,
                LanguageCode = DefaultLanguageCode,
                UserLookupValue = UserJson.BuildUserLookupValue(input.SystemUserId),
                IsSignedOut = false
            },
            static failure => failure.MapFailureCode(ToUserSignInFailureCode))
        .MapSuccess(
            user => UserJson.BuildDataverseInput(input.SystemUserId, option.BotId, user))
        .ForwardValue(
            dataverseApi.UpdateEntityAsync,
            static failure => failure.WithFailureCode(UserSignInFailureCode.Unknown));

    private Result<UserChatId, Failure<UserSignInFailureCode>> ParseTelegramDataOrFailure(UserSignInIn input)
    {
        var dataArray = Uri.UnescapeDataString(input.TelegramData).Split('&');
        var hash = string.Empty;

        var filteredData = new List<string>(dataArray.Length);
        foreach (var data in dataArray)
        {
            if (data.StartsWith(HashParameterName, StringComparison.InvariantCultureIgnoreCase))
            {
                hash = data[HashParameterName.Length..];
            }
            else
            {
                filteredData.Add(data);
            }
        }

        if (string.IsNullOrEmpty(hash))
        {
            return Failure.Create(UserSignInFailureCode.InvalidTelegramData, "Invalid telegram data");
        }

        filteredData.Sort();

        using var hashAlgorithmWebAppData = new HMACSHA256(Encoding.UTF8.GetBytes(TelegramWebAppData));
        var secretKey = hashAlgorithmWebAppData.ComputeHash(Encoding.UTF8.GetBytes(option.BotToken));

        using var hashAlgorithmSecretKey = new HMACSHA256(secretKey);
        var dataBytes = Encoding.UTF8.GetBytes(string.Join("\n", filteredData));

        var expectedHash = BitConverter.ToString(hashAlgorithmSecretKey.ComputeHash(dataBytes)).Replace("-", string.Empty).ToLowerInvariant();
        if (string.Equals(expectedHash, hash, StringComparison.Ordinal) is false)
        {
            return Failure.Create(UserSignInFailureCode.InvalidTelegramData, "Invalid hash");
        }

        var match = UserIdRegex.Match(filteredData[^1]);
        if (match.Success is false || long.TryParse(match.Groups[1].Value, out var chatId) is false)
        {
            return Failure.Create(UserSignInFailureCode.InvalidTelegramData, "Invalid id");
        }

        return new UserChatId
        {
            ChatId = chatId,
            SystemUserId = input.SystemUserId
        };
    }
}