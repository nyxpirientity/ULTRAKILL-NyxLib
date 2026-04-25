using System.Reflection;

namespace Nyxpiri.ULTRAKILL.NyxLib
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

    public struct FieldAccess<ObjectType, FieldType>
    {
        FieldInfo Fi;

        public FieldAccess(string fieldName, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            Fi = typeof(ObjectType).GetField(fieldName, bindingFlags);
        }

        public FieldType GetValue(ObjectType objectType)
        {
            return (FieldType)Fi.GetValue(objectType);
        }

        public void SetValue(ObjectType objectType, FieldType value)
        {
            Fi.SetValue(objectType, value);
        }
    }
}