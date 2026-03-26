using System;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class UpdateEvents
    {
        public delegate void OnUpdateEventHandler();
        public static event OnUpdateEventHandler OnUpdate = null;
        
        public delegate void OnFixedUpdateEventHandler();
        public static event OnFixedUpdateEventHandler OnFixedUpdate = null;

        public delegate void OnLateUpdateEventHandler();
        public static event OnLateUpdateEventHandler OnLateUpdate = null;

        internal static void NotifyLateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        internal static void NotifyFixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        internal static void NotifyUpdate()
        {
            OnUpdate?.Invoke();
        }
    }
}