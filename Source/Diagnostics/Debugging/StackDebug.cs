using System.Diagnostics;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

public static class StackDebug
{
    public static string GetStackString()
    {
        StackTrace trace = new StackTrace(1, false);
        return $"{trace}";
    }

    public static void PrintStack(bool includeFileInfo = false)
    {
        StackTrace trace = new StackTrace(1, includeFileInfo);
        Log.Message($"Stack debug! {trace}");
    }
}