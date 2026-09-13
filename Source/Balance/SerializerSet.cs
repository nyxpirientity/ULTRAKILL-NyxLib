using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class SerializerSet
{
    public string Serialize(object value)
    {
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

    public void Add<T>(Func<T, string> serializer, Func<string, T?> deserializer) where T : struct
    {
        Assert.IsFalse(ContainsType<T>());
        Assert.IsNotNull(serializer);
        Assert.IsNotNull(deserializer);

        Add(typeof(T),
            obj => obj != null ? serializer.Invoke((T)obj) : "null",
            data => data != "null" ? deserializer.Invoke(data) : null
        );
    }

    public void Add(Type type, Func<object, string> serializer, Func<string, object> deserializer)
    {
        Assert.IsFalse(ContainsType(type));
        Assert.IsNotNull(type);
        Assert.IsNotNull(serializer);
        Assert.IsNotNull(deserializer);

        _serializers.Add(type, serializer);
        _deserializers.Add(type, deserializer);
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

    private Dictionary<Type, Func<object, string>> _serializers = [];
    private Dictionary<Type, Func<string, object>> _deserializers = [];
}