using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/deleteTimesheet", Summary = "Delete timesheet")]
[EndpointTag("Timesheet")]
public interface ITimesheetDeleteFunc
{
    ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        TimesheetDeleteIn input, CancellationToken cancellationToken);
}