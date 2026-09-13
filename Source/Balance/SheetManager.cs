using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class CompositeManager
{
    public static SerializerSet Serializers = new();

    public static T CreateComposite<T>(string id) where T : BalanceComposite, new()
    {
        T comp = new();

        _composites[id] = comp;
        comp.Serializers = Serializers;

        return comp;
    }

    private static Dictionary<string, BalanceComposite> _composites = [];
}