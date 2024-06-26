using System;
using GarageGroup.Infra;
using Microsoft.OpenApi.Any;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class DailyNotificationUserPreference : INotificationUserPreference
{
    public static OpenApiObject GetExample() 
        =>
        new()
        {
            {nameof(WorkedHours), new OpenApiInteger(8)},
            {nameof(NotificationTime), new OpenApiString("18:00:00")},
        };
    
    [SwaggerDescription(DailyUserPreference.WorkedHoursDescription)]
    public int WorkedHours { get; init; }

    [SwaggerDescription(DailyUserPreference.NotificationTimeDescription)]
    public TimeOnly NotificationTime { get; init; }
}