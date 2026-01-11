using System;

namespace UKAIW
{
    public static class UpdateEvents
    {
        public static Action OnUpdate = null;
        public static Action OnFixedUpdate = null;
        public static Action OnLateUpdate = null;
        public static Action OnGUI = null;
    }
}