using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public sealed partial class SubscriptionSetGetFunc : ISubscriptionSetGetFunc
{
    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    private readonly IDataverseEntitySetGetSupplier dataverseApi;

    private readonly SubscriptionSetGetOption option;

    internal SubscriptionSetGetFunc(IDataverseEntitySetGetSupplier dataverseApi, SubscriptionSetGetOption option)
    {
        this.dataverseApi = dataverseApi;
        this.option = option;
    }

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