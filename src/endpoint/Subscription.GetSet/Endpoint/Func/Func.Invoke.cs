using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class SubscriptionSetGetFunc
{
    public ValueTask<Result<SubscriptionSetGetOut, Failure<SubscriptionSetGetFailureCode>>> InvokeAsync(
        SubscriptionSetGetIn input, CancellationToken cancellationToken)
        => 
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => SubscriptionJson.BuildGetInput(@in.SystemUserId, option.BotId))
        .PipeValue(
            dataverseApi.GetEntitySetAsync<SubscriptionJson>)
        .Map(
            static @out  => new SubscriptionSetGetOut
            {
                Subscriptions = @out.Value.Map(MapSubscription)
            },
            static failure => failure.WithFailureCode(SubscriptionSetGetFailureCode.Unknown));

    private static SubscriptionBase MapSubscription(SubscriptionJson subscription)
    {
        var notificationType = subscription.NotificationType?.Key;
        if (string.Equals(notificationType, "dailyTimesheetNotification", StringComparison.InvariantCultureIgnoreCase))
        {
            return GetDailyNotificationSubscription(subscription);
        }

        if (string.Equals(notificationType, "weeklyTimesheetNotification", StringComparison.InvariantCultureIgnoreCase))
        {
            return GetWeeklyNotificationSubscription(subscription);
        }

        throw new InvalidOperationException($"An unknown notification type: '{notificationType}'");
    }

    private static DailyNotificationSubscription GetDailyNotificationSubscription(SubscriptionJson subscription)
    {
        var preference = DeserializeUserPreference<DailyNotificationUserPreferenceJson>(subscription.UserPreference);
        if (preference is null)
        {
            return new(default);
        }

        return new(
            userPreference: new(preference.WorkedHours, preference.NotificationTime));
    }

    private static WeeklyNotificationSubscription GetWeeklyNotificationSubscription(SubscriptionJson subscription)
    {
        var preference = DeserializeUserPreference<WeeklyNotificationUserPreferenceJson>(subscription.UserPreference);
        if (preference is null)
        {
            return new(null);
        }

        return new(
            userPreference: new(
                weekday: ParseWeekdays(preference.Weekday),
                workedHours: preference.WorkedHours,
                notificationTime: preference.NotificationTime));

        static FlatArray<Weekday> ParseWeekdays(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            return value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Enum.Parse<Weekday>).ToFlatArray();
        }
    }
}