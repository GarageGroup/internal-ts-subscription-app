using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProjectSetSearchIn
{
    public ProjectSetSearchIn(
        [ClaimIn] Guid systemUserId,
        [JsonBodyIn] [AllowNull] string searchText,
        [JsonBodyIn, IntegerExample(50)] int? top)
    {
        SystemUserId = systemUserId;
        SearchText = searchText.OrEmpty();
        Top = top;
    }

    public Guid SystemUserId { get; }

    public string SearchText { get; }

    public int? Top { get; }
}