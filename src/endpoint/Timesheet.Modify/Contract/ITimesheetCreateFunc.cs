using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetModifyMetadata;

[Endpoint(EndpointMethod.Post, Func.RouteCreate, Summary = Func.SummaryCreate, Description = Func.DescriptionCreate)]
[EndpointTag(Func.Tag)]
public interface ITimesheetCreateFunc
{
    ValueTask<Result<Unit, Failure<TimesheetCreateFailureCode>>> InvokeAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken);
}