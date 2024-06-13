using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class UserSignInFunc(IDataverseEntityCreateSupplier dataverseApi, UserSignInOption option) : IUserSignInFunc
{
    private const string DefaultLanguageCode = "en";
}