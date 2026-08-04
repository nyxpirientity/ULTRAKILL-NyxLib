using System.Reflection;

namespace Nyxpiri.ULTRAKILL.NyxLib
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

    public struct PropertyAccess<ObjectType, PropertyType>
    {
        PropertyInfo Pi;

        public PropertyAccess(string propertyName, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            Pi = typeof(ObjectType).GetProperty(propertyName, bindingFlags);
        }

        public PropertyType GetValue(ObjectType objectType)
        {
            return (PropertyType)Pi.GetValue(objectType);
        }

        public void SetValue(ObjectType objectType, PropertyType value)
        {
            Pi.SetValue(objectType, value);
        }
    }
}