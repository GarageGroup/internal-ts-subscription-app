using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static UserSignInMetadata;

public sealed record class UserSignInIn
{
    public UserSignInIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn, SwaggerDescription(In.ChatIdDescription), IntegerExample(In.ChatIdExample)] long chatId)
    {
        SystemUserId = systemUserId;
        ChatId = chatId;
    }

    public Guid SystemUserId { get; }

    public long ChatId { get; }
}