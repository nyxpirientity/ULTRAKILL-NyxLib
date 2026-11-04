using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nyxpiri.ULTRAKILL.NyxLib;

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

public static class FieldAccessExtensions
{
    public static FieldInfo GetFieldInfoFA(Type type, string fieldName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
    {
        if (!_fieldInfos.TryGetValue(type, out var infos))
        {
            infos = [];
            _fieldInfos[type] = infos;
        }

        if (!infos.TryGetValue(fieldName, out var fi))
        {
            fi = type.GetField(fieldName, bindingFlags);
            infos[fieldName] = fi;
        }

        return fi;
    }

    public static void SetFieldFA(this object target, string fieldName, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
    {
        Assert.IsNotNull(target);

        var fi = GetFieldInfoFA(target.GetType(), fieldName, bindingFlags);

        Assert.IsNotNull(fi);

        fi.SetValue(target, value);
    }

    public static T GetFieldFA<T>(this object target, string fieldName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
    {
        Assert.IsNotNull(target);

        var fi = (GetFieldInfoFA(target.GetType(), fieldName, bindingFlags));

        Assert.IsNotNull(fi);

        return (T)fi.GetValue(target);
    }


    private static Dictionary<Type, Dictionary<string, FieldInfo>> _fieldInfos = [];
}