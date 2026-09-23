using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BepInEx;
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
    public IReadOnlyDictionary<string, object> Values => _values;

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

    public string Serialize(IReadOnlyCollection<string> entries, IReadOnlyDictionary<string, object> defaultValues)
    {
        StringBuilder fullDataStr = new();
        StringBuilder descStrBuilder = new();

        BalanceNamespace mainNamespace = new(entries);

        string activeNamespace = null;

        foreach (var entryId in mainNamespace.GetAllEntries())
        {
            bool isStart = fullDataStr.Length == 0;

            if (!isStart)
            {
                fullDataStr.Append("\n");
            }

            object value = _values.GetValueOrDefault(entryId, null);
            BalanceEntry entry = Entries[entryId];
            Type type = entry.Type;
            string strVal = Serializers.Serialize(value);
            descStrBuilder.Clear();
            string typeDesc = Serializers.GetTypeDescription(entry.Type);
            string description = $"{(entry.Description.IsNullOrWhiteSpace() ? "No description added" : entry.Description)}";
            string typeInfo = $"{entry.Type.FullName}{(typeDesc.IsNullOrWhiteSpace() ? "" : $" ({typeDesc})")}";

            if (strVal == null)
            {
                throw new NotImplementedException();
            }

            int dotIdx = entry.ID.LastIndexOf('.');

            if (dotIdx >= 0)
            {
                string entryNamespace = entry.ID[..dotIdx];
                string entryName = entry.ID[(dotIdx + 1)..];

                if (activeNamespace != entryNamespace)
                {
                    activeNamespace = entryNamespace;
                    fullDataStr.Append($"{(isStart ? "" : "\n")}[{activeNamespace}]\n");
                }

                string defaultValueStr = "Failed to resolve default value.";

                if (defaultValues.TryGetValue(entry.ID, out var defVal))
                {
                    string potentialDefaultValueStr = Serializers.Serialize(defVal);

                    if (potentialDefaultValueStr != null)
                    {
                        defaultValueStr = potentialDefaultValueStr;
                    }
                }

                fullDataStr.Append($"{entryName} = {strVal} // {typeInfo} - Default = {defaultValueStr} - {description}");
            }
            else
            {
                if (!(activeNamespace.IsNullOrWhiteSpace()))
                {
                    fullDataStr.Append($"{(isStart ? "" : "\n")}[]\n");
                }

                fullDataStr.Append($"{entry.ID} = {strVal} // {typeInfo} - {description}");
            }
        }

        return fullDataStr.ToString();
    }

    public void Deserialize(string data)
    {
        StringQueue queue = new(data);

        char[] spaces = [' ', '\t'];

        string prefix = "";

        while (!queue.Finished)
        {
            string line = queue.PopUntil(['\n'], out char? s);
            queue.Skip();
            line = line.TrimStart();

            if (line.StartsWith('#'))
            {
                continue;
            }

            if (line.IsNullOrWhiteSpace())
            {
                continue;
            }

            if (line.StartsWith('['))
            {
                line = line.TrimEnd();

                if (line.EndsWith(']'))
                {
                    line = line[1..^1];
                    prefix = $"{line}.";
                }
                else
                {
                    throw new FormatException($"Invalid line format {line} (starts with '[' but doesn't end with ']')");
                }
            }
            else if (line.Contains("="))
            {
                StringQueue lineQueue = new(line);

                string name = lineQueue.PopUntil(['='], out _).Trim();
                lineQueue.Skip();

                if (name.Any((c) => spaces.Contains(c)))
                {
                    Log.Warning($"BalanceSheet line '{line}' seemingly contains invalid variable id due to spaces being in the variable.");
                    continue;
                }

                name = $"{prefix}{name}";

                string valueStr = lineQueue.GetRemaining();

                int userHints = valueStr.LastIndexOf("//");

                if (userHints >= 0)
                {
                    valueStr = valueStr[0..userHints];
                }

                if (!Entries.TryGetValue(name, out var entry))
                {
                    Log.Warning($"Unknown BalanceSheet entry '{name}' with value '{valueStr}'");
                    continue;
                }

                valueStr = valueStr.Trim();

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
    }

    public void Save(string path, IReadOnlyCollection<string> entries, IReadOnlyDictionary<string, object> defaultValues)
    {
        string data = Serialize(entries, defaultValues);

        string directory = path.Substring(0, path.TrimEnd(['/', '\\']).LastIndexOfAny(['/', '\\']));
        Directory.CreateDirectory(directory);
        using StreamWriter file = File.CreateText(path);
        file.Write(data);
        file.Close();
    }

    private IReadOnlyDictionary<string, BalanceEntry> Entries = null;
    private Dictionary<string, object> _values = [];
}