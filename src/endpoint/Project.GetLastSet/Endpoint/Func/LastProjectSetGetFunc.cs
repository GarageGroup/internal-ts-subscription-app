using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class LastProjectSetGetFunc(ISqlQueryEntitySetSupplier sqlApi, ITodayProvider todayProvider, LastProjectSetGetOption option) : ILastProjectSetGetFunc
{
    private static readonly DbParameterArrayFilter AllowedProjectTypeSetFilter;

    private static readonly DbRawFilter IncidentStateCodeFilter;

    static LastProjectSetGetFunc()
    {
        AllowedProjectTypeSetFilter = DbLastProject.BuildAllowedProjectTypeSetFilter();
        IncidentStateCodeFilter = DbLastProject.BuildIncidentStateCodeFilter();
    }
}