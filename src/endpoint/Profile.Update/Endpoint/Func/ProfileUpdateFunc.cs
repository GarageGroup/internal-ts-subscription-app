using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProfileUpdateFunc(IDataverseEntityUpdateSupplier dataverseApi, IBotInfoGetSupplier botApi) : IProfileUpdateFunc
{
}