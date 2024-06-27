using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;
using Microsoft.OpenApi.Any;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class DailyNotificationUserPreference : INotificationUserPreference
{
    internal static OpenApiObject Example { get; }
        =
        new()
        {
            [NamingPolicy.ConvertName(nameof(WorkedHours))] = new OpenApiInteger(In.DailyNotificationWorkedHoursExample),
            [NamingPolicy.ConvertName(nameof(NotificationTime))] = new OpenApiString(In.NotificationTimeExample)
        };

    public DailyNotificationUserPreference(decimal workedHours, [AllowNull] string notificationTime)
    {
        WorkedHours = workedHours;
        NotificationTime = notificationTime.OrEmpty();
    }

    [SwaggerDescription(In.DailyNotificationWorkedHoursDescription)]
    public decimal WorkedHours { get; }

    [SwaggerDescription(In.NotificationTimeDescription)]
    public string NotificationTime { get; }
}