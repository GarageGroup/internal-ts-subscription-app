namespace GarageGroup.Internal.Timesheet;

internal sealed record class CacheValue
{
    public required long Id { get; init; }

    public required string? Username { get; init; }
}