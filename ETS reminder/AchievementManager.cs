using Microsoft.Toolkit.Uwp.Notifications;

namespace ETS_reminder;

public record Achievement(
    string Id,
    string Name,
    string Description,
    string Icon,
    AchievementCategory Category);

public enum AchievementCategory
{
    Milestone,
    Streak,
    Special
}

public static class AchievementManager
{
    public static readonly Achievement[] AllAchievements =
    [
        // Milestones
        new("first_report",   "First Step",       "File your first ETS report",              "&#x2705;", AchievementCategory.Milestone),
        new("reports_10",     "Getting Started",   "File 10 reports",                         "&#x1F4DD;", AchievementCategory.Milestone),
        new("reports_25",     "Quarter Century",   "File 25 reports",                         "&#x1F4CB;", AchievementCategory.Milestone),
        new("reports_50",     "Half Century",      "File 50 reports",                         "&#x1F4DA;", AchievementCategory.Milestone),
        new("reports_100",    "Centurion",         "File 100 reports",                        "&#x1F4AF;", AchievementCategory.Milestone),
        new("reports_200",    "Double Century",    "File 200 reports",                        "&#x1F3C5;", AchievementCategory.Milestone),
        new("reports_500",    "Report Machine",    "File 500 reports",                        "&#x2699;",  AchievementCategory.Milestone),

        // Streaks
        new("streak_3",      "Warming Up",         "Reach a 3-day streak",                   "&#x1F31F;", AchievementCategory.Streak),
        new("streak_5",      "On Fire",            "Reach a 5-day streak",                   "&#x1F525;", AchievementCategory.Streak),
        new("streak_10",     "Unstoppable",        "Reach a 10-day streak",                  "&#x26A1;",  AchievementCategory.Streak),
        new("streak_20",     "Legendary",          "Reach a 20-day streak",                  "&#x1F3C6;", AchievementCategory.Streak),
        new("streak_50",     "Iron Will",          "Reach a 50-day streak",                  "&#x1F48E;", AchievementCategory.Streak),
        new("streak_100",    "Unbreakable",        "Reach a 100-day streak",                 "&#x1F451;", AchievementCategory.Streak),

        // Special
        new("week_perfect",  "Perfect Week",       "File all 5 reports in a week",           "&#x2B50;",  AchievementCategory.Special),
        new("month_perfect", "Perfect Month",      "File every weekday report in a month",   "&#x1F31D;", AchievementCategory.Special),
        new("early_bird",    "Early Bird",         "File a report before 10:00 AM",          "&#x1F425;", AchievementCategory.Special),
        new("night_owl",     "Night Owl",          "File a report after 8:00 PM",            "&#x1F989;", AchievementCategory.Special),
        new("coins_100",     "Coin Collector",     "Earn 100 coins",                         "&#x1F4B0;", AchievementCategory.Special),
        new("coins_500",     "Rich",               "Earn 500 coins",                         "&#x1F4B0;", AchievementCategory.Special),
        new("level_5",       "Halfway There",      "Reach Level 5",                          "&#x1F680;", AchievementCategory.Special),
        new("level_10",      "ETS God",            "Reach Level 10",                         "&#x1F3C6;", AchievementCategory.Special),
    ];

    /// <summary>
    /// Checks all achievements against current stats and unlocks any new ones.
    /// Returns the list of newly unlocked achievements.
    /// </summary>
    public static List<Achievement> CheckAndUnlock(StatsResult stats)
    {
        var profile = UserProfile.Instance;
        if (profile == null) return [];

        var newlyUnlocked = new List<Achievement>();

        foreach (var achievement in AllAchievements)
        {
            if (profile.UnlockedAchievements.Contains(achievement.Id))
                continue;

            if (IsUnlocked(achievement, stats, profile))
            {
                profile.UnlockedAchievements.Add(achievement.Id);
                newlyUnlocked.Add(achievement);
            }
        }

        if (newlyUnlocked.Count > 0)
            profile.Save();

        return newlyUnlocked;
    }

    private static bool IsUnlocked(Achievement a, StatsResult stats, UserProfile profile) => a.Id switch
    {
        // Milestones
        "first_report" => stats.TotalReports >= 1,
        "reports_10" => stats.TotalReports >= 10,
        "reports_25" => stats.TotalReports >= 25,
        "reports_50" => stats.TotalReports >= 50,
        "reports_100" => stats.TotalReports >= 100,
        "reports_200" => stats.TotalReports >= 200,
        "reports_500" => stats.TotalReports >= 500,

        // Streaks
        "streak_3" => stats.LongestStreak >= 3,
        "streak_5" => stats.LongestStreak >= 5,
        "streak_10" => stats.LongestStreak >= 10,
        "streak_20" => stats.LongestStreak >= 20,
        "streak_50" => stats.LongestStreak >= 50,
        "streak_100" => stats.LongestStreak >= 100,

        // Special
        "week_perfect" => CheckPerfectWeek(),
        "month_perfect" => CheckPerfectMonth(),
        "early_bird" => CheckTimeOfDay(0, 10),
        "night_owl" => CheckTimeOfDay(20, 24),
        "coins_100" => stats.TotalCoins >= 100,
        "coins_500" => stats.TotalCoins >= 500,
        "level_5" => profile.Level >= 5,
        "level_10" => profile.Level >= 10,

        _ => false
    };

    private static bool CheckPerfectWeek()
    {
        var allDates = ReportStorage.GetAllReportDates()
            .Select(DateOnly.FromDateTime)
            .ToHashSet();

        var today = DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, App.AppTimeZone));

        // Check current week (Mon-Fri)
        var monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        if (today.DayOfWeek == DayOfWeek.Sunday)
            monday = monday.AddDays(-7);

        for (int i = 0; i < 5; i++)
        {
            var weekday = monday.AddDays(i);
            if (weekday > today) return false;
            if (!allDates.Contains(weekday)) return false;
        }
        return true;
    }

    private static bool CheckPerfectMonth()
    {
        var allDates = ReportStorage.GetAllReportDates()
            .Select(DateOnly.FromDateTime)
            .ToHashSet();

        var today = DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, App.AppTimeZone));

        // Check previous month (must be complete)
        var firstOfLastMonth = new DateOnly(today.Month == 1 ? today.Year - 1 : today.Year,
            today.Month == 1 ? 12 : today.Month - 1, 1);
        var lastOfLastMonth = firstOfLastMonth.AddMonths(1).AddDays(-1);

        var d = firstOfLastMonth;
        while (d <= lastOfLastMonth)
        {
            if (d.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
            {
                if (!allDates.Contains(d)) return false;
            }
            d = d.AddDays(1);
        }
        return true;
    }

    private static bool CheckTimeOfDay(int fromHour, int toHour)
    {
        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, App.AppTimeZone);
        return now.Hour >= fromHour && now.Hour < toHour;
    }

    /// <summary>
    /// Shows a Windows toast notification for a newly unlocked achievement.
    /// </summary>
    public static void ShowAchievementToast(Achievement achievement)
    {
        new ToastContentBuilder()
            .AddArgument("action", "achievement")
            .AddText("Achievement Unlocked!")
            .AddText($"{achievement.Name}")
            .AddText(achievement.Description)
            .Show();
    }
}
