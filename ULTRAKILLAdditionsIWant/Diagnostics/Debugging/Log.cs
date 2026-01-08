using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKAIW.Diagnostics.Debug
{
    public static class Log
    {
        public enum Level
        {
            Unexpected,
            Unlikely,
            Likely,
            Expected,
            Performance,
            TraceExpected,
        }

        public static void Info(Level level, string message)
        {
            switch (level)
            {
                case Level.Performance:
                    if (!Options.IncludePerformanceLogs)
                    {
                        return;
                    }
                    MelonLogger.Msg(System.Drawing.Color.Green, $"[PERF] {message}");
                    return;
                case Level.TraceExpected:
                    if (!Options.IncludeTraceExpectedLogs)
                    {
                        return;
                    }
                    MelonLogger.Msg(System.Drawing.Color.DarkGray, $"[TE:INFO]{message}");
                    return;
                case Level.Expected:
                    if (!Options.IncludeExpectedLogs)
                    {
                        return;
                    }
                    MelonLogger.Msg(System.Drawing.Color.DarkGray, $"[EX:INFO]{message}");
                    return;
                case Level.Likely:
                    if (!Options.IncludeLikelyLogs)
                    {
                        return;
                    }
                    MelonLogger.Msg(System.Drawing.Color.Gray, $"[LI:INFO]{message}");
                    return;
                case Level.Unlikely:
                    if (!Options.IncludeUnlikelyLogs)
                    {
                        return;
                    }
                    MelonLogger.Msg($"[UL:INFO]{message}");
                    return;
                case Level.Unexpected:
                    if (!Options.IncludeUnexpectedLogs)
                    {
                        return;
                    }
                    MelonLogger.Msg(System.Drawing.Color.LightYellow, $"[UE:INFO]{message}");
                    return;
            }
        }

        public static void Warning(string message)
        {
            MelonLogger.Warning(message);
        }

        public static void Error(string message)
        {
            MelonLogger.Error(message);
        }

        public static void PerformanceInfo(string message)
        {
            Info(Level.Performance, message);
        }

        public static void TraceExpectedInfo(string message)
        {
            Info(Level.TraceExpected, message);
        }

        public static void ExpectedInfo(string message)
        {
            Info(Level.Expected, message);
        }

        public static void LikelyInfo(string message)
        {
            Info(Level.Likely, message);
        }

        public static void UnlikelyInfo(string message)
        {
            Info(Level.Unlikely, message);
        }

        public static void UnexpectedInfo(string message)
        {
            Info(Level.Unexpected, message);
        }
    }
}
