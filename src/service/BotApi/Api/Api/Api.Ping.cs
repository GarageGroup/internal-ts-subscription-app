using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class BotApiImpl
{
    public ValueTask<Result<Unit, Failure<Unit>>> PingAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            UpdateCacheValueAsync)
        .MapSuccess(
            Unit.From);
}