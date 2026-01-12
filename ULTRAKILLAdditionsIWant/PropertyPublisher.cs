using System;
using System.Reflection;

namespace UKAIW
{
    public class PropertyPublisher<IT, VT> where IT : class
    {
        PropertyInfo Fi = null;
        IT Instance = null;
        
        public VT Value
        {
            get => (VT)Fi.GetValue(Instance);
            set => Fi.SetValue(Instance, value);
        }

        public PropertyPublisher(IT instance, string fieldName)
        {
            Instance = instance;
            Fi = typeof(IT).GetProperty(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}