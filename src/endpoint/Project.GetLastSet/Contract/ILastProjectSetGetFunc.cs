using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/lastProjects", Description = "Get last projects, leads, opportunities and incidents")]
[EndpointTag("Project")]
public interface ILastProjectSetGetFunc
{
    ValueTask<Result<LastProjectSetGetOut, Failure<LastProjectSetGetFailureCode>>> InvokeAsync(
        LastProjectSetGetIn input, CancellationToken cancellationToken);
}