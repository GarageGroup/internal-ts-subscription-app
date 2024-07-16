using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProfileGetFunc(ISqlQueryEntitySupplier sqlApi, IBotInfoGetSupplier botApi) : IProfileGetFunc
{
}