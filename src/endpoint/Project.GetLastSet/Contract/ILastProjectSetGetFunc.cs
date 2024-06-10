using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/getLastProjects", Description = "Get last user projects, leads, opportunities and incidents")]
[EndpointTag("Project")]
public interface ILastProjectSetGetFunc
{
    ValueTask<Result<LastProjectSetGetOut, Failure<Unit>>> InvokeAsync(
        LastProjectSetGetIn input, CancellationToken cancellationToken);
}