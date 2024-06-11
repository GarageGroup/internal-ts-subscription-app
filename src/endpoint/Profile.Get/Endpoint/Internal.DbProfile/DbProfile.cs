using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_telegram_bot_user", AliasName)]
public sealed partial record class DbProfile : IDbEntity<DbProfile>
{
    private const string All = "QueryAll";

    private const string AliasName = "p";
}