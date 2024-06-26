using System;
using System.Linq;
using GarageGroup.Infra;
using Microsoft.OpenApi.Any;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class WeeklyNotificationUserPreference : INotificationUserPreference
{
    public static OpenApiObject GetExample()
    {
        var weekdayExample = new OpenApiArray();
        weekdayExample.Add(new OpenApiString(Timesheet.Weekday.Friday.ToString()));
        weekdayExample.Add(new OpenApiString(Timesheet.Weekday.Saturday.ToString()));
        weekdayExample.Add(new OpenApiString(Timesheet.Weekday.Sunday.ToString()));
        
        return new()
        {
            {nameof(WorkedHours), new OpenApiInteger(40)},
            {nameof(NotificationTime), new OpenApiString("18:00:00")},
            {nameof(Weekday), weekdayExample}
        };
    }
    
    [SwaggerDescription(WeeklyUserPreference.WeekdaysTimeDescription)]
    public FlatArray<Weekday> Weekday { get; init; }

    [SwaggerDescription(WeeklyUserPreference.WorkedHoursDescription)]
    public int WorkedHours { get; init; }

    [SwaggerDescription(WeeklyUserPreference.NotificationTimeDescription)]
    public TimeOnly NotificationTime { get; init; }
}
