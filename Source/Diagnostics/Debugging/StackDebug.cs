using System.Diagnostics;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

public static class StackDebug
{
    public static void PrintStack()
    {
        StackTrace trace = new StackTrace(1, false);
        Log.DebugInfo($"{trace}");
    }
}