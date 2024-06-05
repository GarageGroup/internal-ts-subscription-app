using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/getTimesheets", Description = "Get timesheets")]
[EndpointTag("Timesheet")]
public interface ITimesheetGetSetFunc
{
    ValueTask<Result<TimesheetGetSetOut, Failure<Unit>>> InvokeAsync(
        TimesheetGetSetIn input, CancellationToken cancellationToken);
}