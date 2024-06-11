using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/getProfile", Description = "Get profile")]
[EndpointTag("Profile")]
public interface IProfileGetFunc
{
    ValueTask<Result<ProfileGetOut, Failure<ProfileGetFailureCode>>> InvokeAsync(
        ProfileGetIn input, CancellationToken cancellationToken);
}