using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class LastProjectSetGetFunc
{
    public ValueTask<Result<LastProjectSetGetOut, Failure<Unit>>> InvokeAsync(
        LastProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => DbLastProject.QueryAll with
            {
                Top = @in.Top,
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbLastProject.BuildOwnerFilter(@in.SystemUserId),
                        DbLastProject.BuildMinDateFilter(GetLastDaysPeriod()),
                        AllowedProjectTypeSetFilter,
                        IncidentStateCodeFilter
                    ]
                },
                Orders = DbLastProject.DefaultOrders
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbLastProject>)
        .MapSuccess(
            static success => new LastProjectSetGetOut
            {
                Projects = success.Map(MapProject)
            });

    private static ProjectItem MapProject(DbLastProject dbTimesheetProject)
        =>
        new(
            id: dbTimesheetProject.ProjectId,
            name: dbTimesheetProject.Subject.OrNullIfEmpty() ?? dbTimesheetProject.ProjectName,
            type: GetProjectType(dbTimesheetProject.ProjectTypeCode));

    private static ProjectType GetProjectType(int projectTypeCode)
        =>
        projectTypeCode switch
        {
            4 => ProjectType.Lead,
            3 => ProjectType.Opportunity,
            112 => ProjectType.Incident,
            _ => ProjectType.Project
        };

    private DateOnly GetLastDaysPeriod()
        =>
        todayProvider.Today.AddDays(-option.LastDaysPeriod);
}