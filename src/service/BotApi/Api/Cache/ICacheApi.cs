using System;

namespace GarageGroup.Internal.Timesheet;

internal interface ICacheApi
{
    CacheValue? GetValue();

    Unit SetValue(CacheValue value);
}