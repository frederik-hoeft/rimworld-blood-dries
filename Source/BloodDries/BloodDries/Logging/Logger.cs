namespace BloodDries.Logging;

internal static class Logger
{
    public static void Log(string message)
    {
        if (BloodDriesMod.Settings.enableLogging)
        {
            Verse.Log.Message($"[{nameof(BloodDries)}] {message}");
        }
    }

    public static void LogVerbose(string message)
    {
        if (BloodDriesMod.Settings.enableVerboseLogging)
        {
            Log(message);
        }
    }

    public static void Error(string message) =>
        Verse.Log.Error($"[{nameof(BloodDries)}] {message}");

    public static void LogAlways(string message) =>
        Verse.Log.Message($"[{nameof(BloodDries)}] {message}");
}
