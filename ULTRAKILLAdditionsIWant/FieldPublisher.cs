using System;
using System.Reflection;

namespace UKAIW
{
    public class FieldPublisher<IT, VT> where IT : class
    {
        FieldInfo Fi = null;
        IT Instance = null;
        
        public VT Value
        {
            get => (VT)Fi.GetValue(Instance);
            set => Fi.SetValue(Instance, value);
        }

        public FieldPublisher(IT instance, string fieldName)
        {
            Instance = instance;
            Fi = typeof(IT).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}