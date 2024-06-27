using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;
using Microsoft.OpenApi.Any;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class WeeklyNotificationUserPreference : INotificationUserPreference
{
    internal static OpenApiObject Example { get; }
        =
        new()
        {
            [NamingPolicy.ConvertName(nameof(WorkedHours))] = new OpenApiInteger(In.WeeklyNotificationWorkedHoursExample),
            [NamingPolicy.ConvertName(nameof(NotificationTime))] = new OpenApiString(In.NotificationTimeExample),
            [NamingPolicy.ConvertName(nameof(Weekday))] = new OpenApiArray
            {
                new OpenApiString(Timesheet.Weekday.Friday.ToString()),
                new OpenApiString(Timesheet.Weekday.Saturday.ToString()),
                new OpenApiString(Timesheet.Weekday.Sunday.ToString())
            }
        };

    public WeeklyNotificationUserPreference(
        FlatArray<Weekday> weekday,
        decimal workedHours,
        [AllowNull] string notificationTime)
    {
        Weekday = weekday;
        WorkedHours = workedHours;
        NotificationTime = notificationTime.OrEmpty();
    }

    [SwaggerDescription(In.WeeklyNotificationWeekdayDescription)]
    public FlatArray<Weekday> Weekday { get; }

    [SwaggerDescription(In.WeeklyNotificationWorkedHoursDescription)]
    public decimal WorkedHours { get; }

    [SwaggerDescription(In.NotificationTimeDescription)]
    public string NotificationTime { get; }
}
