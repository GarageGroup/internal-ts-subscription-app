using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/updateTimesheet", Description = "Update timesheet")]
[EndpointTag("Timesheet")]
public interface ITimesheetUpdateFunc
{
    ValueTask<Result<Unit, Failure<TimesheetUpdateFailureCode>>> InvokeAsync(
        TimesheetUpdateIn input, CancellationToken cancellationToken);
}