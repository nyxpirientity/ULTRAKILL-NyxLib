using System;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class TryLog
    {
        public static void Action(Action action)
        {
            try
            {            
                action.Invoke();
            }
            catch (System.Exception e)
            {
                Log.Error($"Exception caught! :c\n{e}");
                throw;
            }
        }
    }
}