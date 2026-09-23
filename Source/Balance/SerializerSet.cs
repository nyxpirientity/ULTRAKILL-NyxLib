using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class SerializerSet
{
    public string GetTypeDescription(object value) => (GetTypeDescription(value.GetType()));

    public string GetTypeDescription(Type type)
    {
        if (!_descriptions.TryGetValue(type, out var desc))
        {
            return null;
        }

        return desc;
    }


    public string Serialize(object value)
    {
        if (value == null)
        {
            return "null";
        }

        if (!_serializers.TryGetValue(value.GetType(), out var serialize))
        {
            return null;
        }

        return serialize(value);
    }

    public object Deserialize(Type type, string data)
    {
        if (!_deserializers.TryGetValue(type, out var deserialize))
        {
            return null;
        }

        return deserialize(data);
    }

    public void Add<T>(Func<T, string> serializer, Func<string, T?> deserializer, string description) where T : struct
    {
        Assert.IsFalse(ContainsType<T>());
        Assert.IsNotNull(serializer);
        Assert.IsNotNull(deserializer);

        Add(typeof(T),
            obj => obj != null ? serializer.Invoke((T)obj) : "null",
            data => data != "null" ? deserializer.Invoke(data) : null,
            description
        );
    }

    public void Add(Type type, Func<object, string> serializer, Func<string, object> deserializer, string description)
    {
        Assert.IsFalse(ContainsType(type));
        Assert.IsNotNull(type);
        Assert.IsNotNull(serializer);
        Assert.IsNotNull(deserializer);

        _serializers.Add(type, serializer);
        _deserializers.Add(type, deserializer);
        _descriptions.Add(type, description);
    }

    public bool ContainsType<T>() => ContainsType(typeof(T));

    public bool ContainsType(Type type)
    {
        if (!_serializers.TryGetValue(type, out var func))
        {
            return false;
        }

        if (func == null)
        {
            return false;
        }

        return true;
    }

    private Dictionary<Type, string> _descriptions = [];
    private Dictionary<Type, Func<object, string>> _serializers = [];
    private Dictionary<Type, Func<string, object>> _deserializers = [];
}