using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class UserSignInIn
{
    public UserSignInIn(
        [ClaimIn] Guid systemUserId,
        [FormBodyIn] long chatId)
    {
        SystemUserId = systemUserId;
        ChatId = chatId;
    }        

    public Guid SystemUserId { get; }

    public long ChatId { get; }
}