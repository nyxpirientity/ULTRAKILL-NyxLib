using System;
using UKAIW.Diagnostics.Debug;

namespace UKAIW
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