using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/searchProjects", Summary = "Search for projects, leads, opportunities and incidents")]
[EndpointTag("Project")]
public interface IProjectSetSearchFunc
{
    ValueTask<Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>> InvokeAsync(
        ProjectSetSearchIn input, CancellationToken cancellationToken);
}