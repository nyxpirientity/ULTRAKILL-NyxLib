using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class BalanceNamespace
{
    public BalanceNamespace() { }
    public BalanceNamespace(IReadOnlyCollection<string> entries)
    {
        Log.Message($"{entries.Count}");
        foreach (var id in entries)
        {
            string[] splitId = id.Split('.');

            if (splitId.Length <= 1)
            {
                Entries.Add(id);
                continue;
            }

            var ns = ResolveNamespace(splitId, 0);

            ns.Entries.Add(id);
        }
    }

    public string Name = "";
    public BalanceNamespace Parent = null;
    public List<BalanceNamespace> Children = [];
    public List<string> Entries = [];

    public IEnumerable<string> GetAllEntries()
    {
        foreach (var entry in Entries)
        {
            yield return entry;
        }

        foreach (var child in Children)
        {
            foreach (var entry in child.GetAllEntries())
            {
                yield return entry;
            }
        }

        yield break;
    }

    public BalanceNamespace GetNamespace(string name)
    {
        BalanceNamespace ns = Children.Find((ns) => ns.Name == name);

        if (ns == null)
        {
            ns = new BalanceNamespace();
            ns.Name = name;
            ns.Parent = this;
            Children.Add(ns);
        }

        return ns;
    }

    string FullName
    {
        get
        {
            if (Parent == null)
            {
                return Name;
            }
            else
            {
                string parentName = Parent.FullName;

                if (parentName.IsNullOrWhiteSpace())
                {
                    return Name;
                }
                else
                {
                    return $"{parentName}.{Name}";
                }
            }
        }
    }

    private BalanceNamespace ResolveNamespace(string[] splitId, int idx)
    {
        BalanceNamespace ns = GetNamespace(splitId[idx]);

        idx += 1;

        if (idx == splitId.Length - 1)
        {
            return ns;
        }

        return ns.ResolveNamespace(splitId, idx);
    }
}