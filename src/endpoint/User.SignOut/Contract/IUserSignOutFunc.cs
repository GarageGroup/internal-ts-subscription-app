using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/signOut", Description = "Log out the user")]
[EndpointTag("Authorization")]
public interface IUserSignOutFunc
{
    ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        UserSignOutIn input, CancellationToken cancellationToken);
}