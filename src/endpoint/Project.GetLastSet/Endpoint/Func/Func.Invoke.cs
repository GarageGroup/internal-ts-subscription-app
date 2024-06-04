using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class LastProjectSetGetFunc
{
    public ValueTask<Result<LastProjectSetGetOut, Failure<LastProjectSetGetFailureCode>>> InvokeAsync(
        LastProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbLastProject.QueryAll with
            {
                Top = @in.Top,
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbLastProject.BuildOwnerFilter(@in.SystemUserId),
                        DbLastProject.BuildMinDateFilter(@in.MinDate),
                        AllowedProjectTypeSetFilter,
                        IncidentStateCodeFilter
                    ]
                },
                Orders = DbLastProject.DefaultOrders
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbLastProject>)
        .Map(
            static success => new LastProjectSetGetOut
            {
                Projects = success.Map(MapProject)
            },
            static failure => failure.WithFailureCode(LastProjectSetGetFailureCode.Unknown));

    private static ProjectItem MapProject(DbLastProject dbTimesheetProject)
        =>
        new(
            id: dbTimesheetProject.ProjectId,
            name: dbTimesheetProject.Subject.OrNullIfEmpty() ?? dbTimesheetProject.ProjectName,
            type: (ProjectType)dbTimesheetProject.ProjectTypeCode);
}