using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/getTags", Description = "Get tags")]
[EndpointTag("Tag")]
public interface ITagSetGetFunc
{
    ValueTask<Result<TagSetGetOut, Failure<Unit>>> InvokeAsync(
        TagSetGetIn input, CancellationToken cancellationToken);
}