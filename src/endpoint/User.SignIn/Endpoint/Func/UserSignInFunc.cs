using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class UserSignInFunc(IDataverseApiClient dataverseApi, UserSignInOption option) : IUserSignInFunc
{
    private const string DefaultLanguageCode = "en";
}