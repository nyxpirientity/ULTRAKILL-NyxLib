using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class BalanceComposite
{
    public BalanceComposite(SerializerSet serializers)
    {
        _defaultSheet = CreateSheet(DefaultSheetID, serializers);
        Serializers = serializers;
    }

    public SerializerSet Serializers
    {
        set
        {
            Assert.IsNotNull(value, $"Serializers for BalanceComposite must not be null");

            foreach (var sheet in Sheets.Values)
            {
                sheet.Serializers = value;
            }
        }
    }

    public BalanceSheet DefaultSheet => _defaultSheet;
    public IReadOnlyDictionary<string, BalanceSheet> Sheets => _sheets;

    public IReadOnlyList<string> Priorities
    {
        set
        {
            _sheetPriorities = value.ToList();
            EvaluatePriorities();
        }
    }

    public IReadOnlyCollection<string> EntryOrder => _entryOrder;

    public const string DefaultSheetID = "default";

    public void SaveDefaultSheet(string path)
    {
        _defaultSheet.Save(path, _entryOrder, _defaultSheet.Values);
    }

    public BalanceEntryRef<T> AddEntry<T>(string id, T defaultValue, string description) where T : struct
    {
        BalanceEntry entry = new(id, description, defaultValue);
        BalanceEntryRef<T> entryRef = new(entry);

        _entries.Add(id, entry);
        _entryOrder.Add(id);

        _defaultSheet.SetValue<T>(id, defaultValue);

        return entryRef;
    }

    public BalanceEntryRef<T> GetEntry<T>(string id)
    {
        return new(_entries[id]);
    }

    public BalanceSheet CreateSheet(string id) => CreateSheet(id, DefaultSheet.Serializers);

    private BalanceSheet CreateSheet(string id, SerializerSet serializers)
    {
        BalanceSheet sheet = new(_entries);
        sheet.Serializers = serializers;

        _sheets.Add(id, sheet);

        foreach (var entry in _entries)
        {
            sheet.SetValue(typeof(object), entry.Key, null);
        }

        _sheetPriorities.Add(id);

        EvaluatePriorities();

        return sheet;
    }

    private void EvaluatePriorities()
    {
        foreach (var entry in _entries)
        {
            BalanceSheet sheet = null;
            string sheetId = null;

            for (int i = 0; i < _sheetPriorities.Count; i++)
            {
                sheetId = _sheetPriorities[i];
                sheet = _sheets[sheetId];

                if (sheet.GetValue(entry.Key, out var val))
                {
                    if (val == null)
                    {
                        continue;
                    }

                    break;
                }
            }

            Assert.IsNotNull(sheet);

            entry.Value.GetterFunc = () => sheet.GetValue(entry.Key);
            entry.Value.ValueSource = sheetId;
        }
    }

    private List<string> _sheetPriorities = [];

    private Dictionary<string, BalanceSheet> _sheets = [];
    private List<string> _entryOrder = [];
    private Dictionary<string, BalanceEntry> _entries = [];

    private BalanceSheet _defaultSheet = null;
}
