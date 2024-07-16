using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed partial class SubscriptionSetGetFunc(IDataverseEntitySetGetSupplier dataverseApi, IBotInfoGetSupplier botApi) : ISubscriptionSetGetFunc
{
    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    private static TUserPreference? DeserializeUserPreference<TUserPreference>(string? json)
        where TUserPreference : class
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonSerializer.Deserialize<TUserPreference>(json, SerializerOptions);
    }
}