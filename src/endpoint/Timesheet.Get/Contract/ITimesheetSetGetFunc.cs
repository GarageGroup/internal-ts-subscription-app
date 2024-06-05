using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/getTimesheet", Description = "Get timesheets")]
[EndpointTag("Timesheet")]
public interface ITimesheetSetGetFunc
{
    ValueTask<Result<TimesheetSetGetOut, Failure<Unit>>> InvokeAsync(
        TimesheetSetGetIn input, CancellationToken cancellationToken);
}