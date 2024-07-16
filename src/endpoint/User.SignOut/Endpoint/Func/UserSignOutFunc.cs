using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class UserSignOutFunc(IDataverseEntityUpdateSupplier dataverseApi, IBotInfoGetSupplier botApi) : IUserSignOutFunc
{
}