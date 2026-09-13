using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class BalanceComposite
{
    public BalanceComposite(SerializerSet serializers)
    {
        _defaultSheet = CreateSheet(DefaultSheetID);
        Serializers = serializers;
    }

    public SerializerSet Serializers { protected get; set; } = null;
    public IReadOnlyDictionary<string, BalanceSheet> Sheets => _sheets;

    public const string DefaultSheetID = "default";

    public BalanceEntryRef<T> AddEntry<T>(string id, T defaultValue, string description) where T : struct
    {
        BalanceEntry entry = new(id, description, defaultValue);
        BalanceEntryRef<T> entryRef = new(entry);

        _entries.Add(id, entry);

        _defaultSheet.SetValue<T>(id, defaultValue);

        return entryRef;
    }

    public BalanceEntryRef<T> GetEntry<T>(string id)
    {
        return new(_entries[id]);
    }

    public BalanceSheet CreateSheet(string id)
    {
        BalanceSheet sheet = new(_entries);

        _sheets.Add(id, sheet);

        foreach (var entry in _entries)
        {
            sheet.SetValue(typeof(object), entry.Key, null);
        }

        SheetPriorities.Add(id);

        EvaluatePriorities();

        return sheet;
    }

    private void EvaluatePriorities()
    {
        foreach (var entry in _entries)
        {
            BalanceSheet sheet = null;
            string sheetId = null;

            for (int i = 0; i < SheetPriorities.Count; i++)
            {
                sheetId = SheetPriorities[i];
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

    private List<string> SheetPriorities = [];

    private Dictionary<string, BalanceSheet> _sheets = [];
    private Dictionary<string, BalanceEntry> _entries = [];

    private BalanceSheet _defaultSheet = null;
}
