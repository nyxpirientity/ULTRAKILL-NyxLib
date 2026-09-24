
using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

[Serializable]
public struct BalanceFloat(BalanceEntryRef<float> reference)
{
    public readonly float Value => UseBase || !_ref.IsValid ? BaseValue : (float)_ref.Value;

    public bool UseBase = false;
    public float BaseValue = default;

    [SerializeField] private BalanceEntryRef _ref = reference.ToRawReference();
}

[Serializable]
public struct BalanceBool(BalanceEntryRef<bool> reference)
{
    public readonly bool Value => UseBase || !_ref.IsValid ? BaseValue : (bool)_ref.Value;

    public bool UseBase = false;
    public bool BaseValue = default;

    [SerializeField] private readonly BalanceEntryRef _ref = reference.ToRawReference();
}

[Serializable]
public struct BalanceInt(BalanceEntryRef<int> reference)
{
    public readonly int Value => UseBase || !_ref.IsValid ? BaseValue : (int)_ref.Value;

    public bool UseBase = false;
    public int BaseValue = default;

    [SerializeField] private readonly BalanceEntryRef _ref = reference.ToRawReference();
}