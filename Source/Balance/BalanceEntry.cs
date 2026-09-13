using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

[Serializable]
public class BalanceEntry
{
    public string ID { get; private set; }
    public string Description;
    public object DefaultValue { get; private set; }

    public Type Type
    {
        get
        {
            Assert.IsNotNull(DefaultValue);
            return DefaultValue.GetType();
        }
    }

    public Func<object> GetterFunc = null;
    public string ValueSource = "";

    public BalanceEntry(string id, string description, object defaultValue)
    {
        ID = id;
        Description = description;
        DefaultValue = defaultValue;
        ResetToDefaultValue();
    }

    public object Value => GetterFunc?.Invoke();
    public bool IsValid => GetterFunc != null;

    public void ResetToDefaultValue()
    {
        GetterFunc = GetDefaultValue;
    }

    private object GetDefaultValue() => DefaultValue;
}

public struct BalanceEntryRef<T>
{
    public BalanceEntryRef(BalanceEntry entry)
    {
        Assert.IsTrue(entry.Type == typeof(T));

        _rawRef = new(entry);
    }

    public bool IsValid => _rawRef.IsValid;

    public readonly T Value => (T)_rawRef.Value;

    public readonly BalanceEntryRef ToRawReference() => _rawRef;

    private BalanceEntryRef _rawRef = default;
}

[Serializable]
public struct BalanceEntryRef(BalanceEntry entry)
{
    public readonly bool IsValid => _entry != null && _entry.IsValid;

    public readonly object Value
    {
        get
        {
            object objVal = _entry.Value;

            Assert.IsNotNull(objVal);

            return objVal;
        }
    }

    [SerializeField, SerializeReference] private readonly BalanceEntry _entry = entry;
}