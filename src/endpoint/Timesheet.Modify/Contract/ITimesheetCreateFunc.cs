using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Post, "/createTimesheet", Description = "Create timesheet")]
[EndpointTag("Timesheet")]
public interface ITimesheetCreateFunc
{
    ValueTask<Result<Unit, Failure<TimesheetCreateFailureCode>>> InvokeAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken);
}