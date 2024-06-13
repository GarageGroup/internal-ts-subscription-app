using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/signIn", Description = "Log in the user")]
[EndpointTag("Authorization")]
public interface IUserSignInFunc
{
    ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        UserSignInIn input, CancellationToken cancellationToken);
}