using System.Diagnostics;
using UKAIW.Diagnostics.Debug;

public static class StackDebug
{
    public static void PrintStack()
    {
        StackTrace trace = new StackTrace(1, false);
        Log.DebugInfo($"{trace}");
    }
}