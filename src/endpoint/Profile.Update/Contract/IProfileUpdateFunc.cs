using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/updateProfile", Description = "Update profile")]
[EndpointTag("Profile")]
public interface IProfileUpdateFunc
{
    ValueTask<Result<Unit, Failure<ProfileUpdateFailureCode>>> InvokeAsync(
        ProfileUpdateIn input, CancellationToken cancellationToken);
}