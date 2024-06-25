using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

[JsonDerivedType(typeof(DailyNotificationUserPreference))]
[JsonDerivedType(typeof(WeeklyNotificationUserPreference))]
public interface INotificationUserPreference;