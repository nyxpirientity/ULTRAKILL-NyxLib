using BepInEx.Logging;

namespace UKAIW.Diagnostics.Debug
{
    internal static class Log
    {
       internal static ManualLogSource Logger = null;

        public static void Warning(string message)
        {
            Logger.LogWarning(message);
        }

        public static void Error(string message)
        {
            Logger.LogError(message);
        }

        public static void PerformanceInfo(string message)
        {
            if (Options.IncludePerformanceLogs)
            {
                Logger.LogDebug(message);
            }
        }

        public static void TraceExpectedInfo(string message)
        {
            if (Options.IncludeTraceExpectedLogs)
            {
                Logger.LogDebug(message);
            }
        }

        public static void DebugInfo(string message)
        {
            Logger.LogDebug(message);
        }

        public static void ExpectedInfo(string message)
        {
            Logger.LogDebug(message);
        }

        public static void LikelyInfo(string message)
        {
            Logger.LogInfo(message);
        }

        public static void UnlikelyInfo(string message)
        {
            Logger.LogInfo(message);
        }

        public static void UnexpectedInfo(string message)
        {
            Logger.LogMessage(message);
        }

        public static void Message(string message)
        {
            Logger.LogMessage(message);
        }
    }
}
