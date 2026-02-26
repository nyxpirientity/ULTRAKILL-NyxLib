using System;

namespace Nyxpiri
{
    public class RegistrationTracker
    {
        public Func<bool> RegisterAction { private get; set; } = null;
        public Func<bool> UnregisterAction { private get; set; } = null;
        public bool Registered
        {
            get
            {
                return _Registered;
            }

            private set => _Registered = value;
        }

        public void Register()
        {
            if (Registered)
            {
                return;
            }

            RegisterAction?.Invoke();
            _Registered = true;
        }

        public void Unregister()
        {
            if (!Registered)
            {
                return;
            }

            UnregisterAction?.Invoke();
            _Registered = false;
        }

        private bool _Registered = false;
    }
}