using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetSetGetFunc
{
    public ValueTask<Result<TimesheetSetGetOut, Failure<Unit>>> InvokeAsync(
         TimesheetSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbTimesheet.QueryAll with
            {
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbTimesheet.BuildOwnerFilter(@in.SystemUserId),
                        DbTimesheet.BuildDateFilter(@in.DateFrom, @in.DateTo),
                        DbTimesheet.AllowedProjectTypeSetFilter
                    ]
                },
                Orders = DbTimesheet.DefaultOrders
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTimesheet>)
        .MapSuccess(
            static success => new TimesheetSetGetOut
            {
                Timesheets = success.Map(MapTimesheet)
            });

    private static TimesheetSetGetItem MapTimesheet(DbTimesheet dbTimesheet)
        =>
        new(
            duration: dbTimesheet.Duration,
            projectId: dbTimesheet.ProjectId,
            projectType: (ProjectType)dbTimesheet.ProjectTypeCode,
            projectName: dbTimesheet.Subject.OrNullIfEmpty() ?? dbTimesheet.ProjectName,
            description: dbTimesheet.Description,
            id: dbTimesheet.Id,
            incidentStateCode: dbTimesheet.IncidentStateCode,
            timesheetStateCode: dbTimesheet.TimesheetStateCode,
            date: DateOnly.FromDateTime(dbTimesheet.Date));
}