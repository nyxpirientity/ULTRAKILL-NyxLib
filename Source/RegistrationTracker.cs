using System;

namespace Nyxpiri
{
    public class RegistrationTracker
    {
        public RegistrationTracker(Func<bool> registerAction, Func<bool> unregisterAction)
        {
            RegisterAction = registerAction;
            UnregisterAction = unregisterAction;
        }

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

            if ((RegisterAction?.Invoke()).GetValueOrDefault(false))
            {
                _Registered = true;
            }
        }

        public void Unregister()
        {
            if (!Registered)
            {
                return;
            }

            if ((UnregisterAction?.Invoke()).GetValueOrDefault(false))
            {
                _Registered = false;
            }
        }

        private bool _Registered = false;
    }
}