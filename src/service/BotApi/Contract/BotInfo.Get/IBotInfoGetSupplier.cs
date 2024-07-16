using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

public interface IBotInfoGetSupplier
{
    ValueTask<Result<BotInfoGetOut, Failure<Unit>>> GetBotInfoAsync(
        Unit input, CancellationToken cancellationToken);
}