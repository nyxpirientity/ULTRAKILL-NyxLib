using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class BalanceSheet
{
    public BalanceSheet(IReadOnlyDictionary<string, BalanceEntry> entries)
    {
        Entries = entries;
    }

    public SerializerSet Serializers = null;

    public void SetValue(Type type, string id, object value)
    {
        if (!Entries.TryGetValue(id, out var entry))
        {
            throw new KeyNotFoundException();
        }

        Assert.IsTrue(entry.Type == type || value is null);

        _values[id] = value;
    }

    public void SetValue<T>(string id, T? value) where T : struct
    {
        if (!Entries.TryGetValue(id, out var entry))
        {
            throw new KeyNotFoundException();
        }

        Assert.IsTrue(entry.Type == typeof(T) || value is null);

        _values[id] = value;
    }

    public object GetValue(string id)
    {
        if (!_values.TryGetValue(id, out var value))
        {
            return null;
        }

        return value;
    }

    public bool GetValue(string id, out object value)
    {
        if (!_values.TryGetValue(id, out value))
        {
            return false;
        }

        return true;
    }

    public string Serialize()
    {
        StringBuilder fullDataStr = new();
        StringBuilder descStrBuilder = new();

        foreach (var pair in _values)
        {
            if (fullDataStr.Length > 0)
            {
                fullDataStr.Append("\n");
            }

            object value = pair.Value;
            BalanceEntry entry = Entries[pair.Key];
            Type type = entry.Type;
            string strVal = Serializers.Serialize(value);
            descStrBuilder.Clear();

            descStrBuilder.Append($"# {entry.Description}\n");
            descStrBuilder.Append($"# type: {entry.Type.FullName}");

            if (strVal == null)
            {
                throw new NotImplementedException();
            }

            fullDataStr.Append($"{descStrBuilder}\n{entry.ID} = {strVal}\n");
        }

        return fullDataStr.ToString();
    }

    public void Deserialize(string data)
    {
        StringQueue queue = new(data);

        char[] spaces = [' ', '\t'];

        while (!queue.Finished)
        {
            string line = queue.PopUntil(['\n'], out char? s);
            line = line.TrimStart(spaces);

            if (line.StartsWith('#'))
            {
                continue;
            }

            StringQueue lineQueue = new(line);

            string name = lineQueue.PopUntil(['='], out _).Trim(spaces);

            if (name.Any((c) => spaces.Contains(c)))
            {
                Log.Warning($"BalanceSheet line '{line}' seemingly contains invalid variable id due to spaces being in the variable.");
                continue;
            }

            string valueStr = lineQueue.GetRemaining().Trim(spaces);

            if (!Entries.TryGetValue(name, out var entry))
            {
                Log.Warning($"Unknown BalanceSheet entry '{name}' with value '{valueStr}'");
                continue;
            }

            object value;

            try
            {
                value = Serializers.Deserialize(entry.Type, valueStr);
            }
            catch (System.Exception e)
            {
                Log.Error($"Error reading value string: '{valueStr}' for variable named: '{name}'. {e.Message}\nFull Line: {line}\nFull Exception: {e}");
                continue;
            }

            SetValue(entry.Type, name, value);
        }
    }

    private IReadOnlyDictionary<string, BalanceEntry> Entries = null;
    private Dictionary<string, object> _values = [];
}