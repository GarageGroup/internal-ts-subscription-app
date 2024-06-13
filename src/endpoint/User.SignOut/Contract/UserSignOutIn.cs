using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class UserSignOutIn
{
    public UserSignOutIn([ClaimIn] Guid systemUserId)
        =>
        SystemUserId = systemUserId;

    public Guid SystemUserId { get; }
}