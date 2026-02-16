using System.Diagnostics;
using MelonLoader;

public static class StackDebug
{
    public static void PrintStack()
    {
        StackTrace trace = new StackTrace(1, false);
        MelonLogger.Msg($"{trace}");
    }
}