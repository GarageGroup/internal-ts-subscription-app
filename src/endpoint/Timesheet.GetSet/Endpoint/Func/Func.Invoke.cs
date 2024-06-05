using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetGetSetFunc
{
    public ValueTask<Result<TimesheetGetSetOut, Failure<Unit>>> InvokeAsync(
         TimesheetGetSetIn input, CancellationToken cancellationToken)
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
                        DbTimesheet.BuildDateFilter(@in.Date),
                        DbTimesheet.AllowedProjectTypeSetFilter
                    ]
                },
                Orders = DbTimesheet.DefaultOrders
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTimesheet>)
        .MapSuccess(
            static success => new TimesheetGetSetOut
            {
                Timesheets = success.Map(MapTimesheet)
            });

    private static TimesheetGetSetItem MapTimesheet(DbTimesheet dbTimesheet)
        =>
        new(
            duration: dbTimesheet.Duration,
            projectId: dbTimesheet.ProjectId,
            projectType: (ProjectType)dbTimesheet.ProjectTypeCode,
            projectName: dbTimesheet.Subject.OrNullIfEmpty() ?? dbTimesheet.ProjectName,
            description: dbTimesheet.Description,
            id: dbTimesheet.Id,
            incidentStateCode: dbTimesheet.IncidentStateCode,
            timesheetStateCode: dbTimesheet.TimesheetStateCode);
}