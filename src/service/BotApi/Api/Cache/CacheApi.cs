using System;
using System.Collections.Concurrent;

namespace GarageGroup.Internal.Timesheet;

internal sealed class CacheApi : ICacheApi
{
    internal static readonly CacheApi Instance;

    private static readonly ConcurrentBag<CacheValue> Cache;

    static CacheApi()
    {
        Instance = new();
        Cache = [];
    }

    public CacheValue? GetValue()
        =>
        Cache.TryPeek(out var value) ? value : default;

    public Unit SetValue(CacheValue value)
        =>
        Unit.Invoke(Cache.Add, value);
}