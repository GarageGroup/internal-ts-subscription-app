using System;
using System.Collections.Generic;
using GarageGroup.Infra;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GarageGroup.Internal.Timesheet;

using static SubscriptionSetGetMetadata;

public sealed record class Subscription : IOpenApiSchemaProvider
{
    public static OpenApiSchema GetSchema(bool nullable, IOpenApiAny? example = null, string? description = null)
        => 
        new()
        {
            Description = description,
            AnyOf = new List<OpenApiSchema>
            {
                new()
                {
                    Title = "Daily notification subscription",
                    Description = "Daily notification subscription",
                    Example = new OpenApiObject
                    {
                        {nameof(Id), new OpenApiString(Out.IdExample)},
                        {nameof(NotificationType), new OpenApiString(nameof(NotificationType.DailyNotification))},
                        {nameof(UserPreference), DailyNotificationUserPreference.GetExample()}
                    }
                },
                new()
                {
                    Title = "Weekly notification subscription",
                    Description = "Weekly notification subscription",
                    Example = new OpenApiObject
                    {
                        {nameof(Id), new OpenApiString(Out.IdExample)},
                        {nameof(NotificationType), new OpenApiString(nameof(NotificationType.WeeklyNotification))},
                        {nameof(UserPreference), WeeklyNotificationUserPreference.GetExample()}
                    }
                }
            }
        };
    
    [SwaggerDescription(Out.IdDescription)]
    public Guid Id { get; init; } 
    
    [SwaggerDescription(Out.NotificationTypeDescription)]
    public NotificationType NotificationType { get; init; }

    [SwaggerDescription(Out.UserPreferenceDescription)]
    public INotificationUserPreference? UserPreference { get; init; }
}