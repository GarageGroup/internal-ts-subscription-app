using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

using static TagSetGetMetadata;

[Endpoint(EndpointMethod.Post, Func.Route, Summary = Func.Summary, Description = Func.Description)]
[EndpointTag(Func.Tag)]
public interface ITagSetGetFunc
{
    ValueTask<Result<TagSetGetOut, Failure<Unit>>> InvokeAsync(
        TagSetGetIn input, CancellationToken cancellationToken);
}