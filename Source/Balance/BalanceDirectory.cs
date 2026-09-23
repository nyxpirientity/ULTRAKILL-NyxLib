using System;
using System.Collections.Generic;
using System.IO;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class BalanceDirectory
{
    public BalanceDirectory(BalanceComposite composite, string path)
    {
        Assert.IsFalse(string.IsNullOrWhiteSpace(path));
        Assert.IsNotNull(composite);

        Composite = composite;
        Path = path;
    }

    public void Load()
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }

        var files = Directory.EnumerateFiles(Path);
        List<string> priority = [];

        foreach (var filePath in files)
        {
            if (!filePath.EndsWith(".balancesheet"))
            {
                continue;
            }

            string fileName = null;

            int lenB = (filePath.LastIndexOf('.'));
            int lenA = filePath.LastIndexOfAny(['/', '\\']);
            try
            {
                fileName = filePath[(lenA + 1)..lenB];
            }
            catch (System.Exception e)
            {
                Log.Error($"exception thrown whilst parsing BalanceSheet path! '{filePath}'; {e}");
                continue;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                continue;
            }

            string data = File.ReadAllText(filePath);
            string sheetId = $"custom.{fileName}";

            if (sheetId == $"custom.{BalanceComposite.DefaultSheetID}")
            {
                continue;
            }

            if (!Composite.Sheets.TryGetValue(sheetId, out var sheet))
            {
                sheet = Composite.CreateSheet(sheetId);
            }

            try
            {
                sheet.Deserialize(data);
            }
            catch (System.Exception e)
            {
                Log.Error($"exception thrown whilst parsing BalanceSheet! {e}");
                continue;
            }

            priority.Add(sheetId);
        }


        string priorityFilePath = $"{Path}/priority.txt";

        if (File.Exists(priorityFilePath))
        {
            using StreamReader priorityFile = File.OpenText(priorityFilePath);
            List<string> filePriority = [];
            for (string line = priorityFile.ReadLine(); line != null; line = priorityFile.ReadLine())
            {
                priority.RemoveAll((s) => s == line);
                filePriority.RemoveAll((s) => s == line);
                filePriority.Add(line);
            }

            priority.InsertRange(0, filePriority);
        }

        priority.RemoveAll((s) => s == BalanceComposite.DefaultSheetID);
        priority.Add(BalanceComposite.DefaultSheetID);

        using StreamWriter priorityFileWriter = File.CreateText(priorityFilePath);

        foreach (var sheetId in priority)
        {
            priorityFileWriter.Write($"{sheetId}\n");

            if (!Composite.Sheets.ContainsKey(sheetId))
            {
                Log.Warning($"Unknown Sheet ID '{sheetId}' in priority list '{priorityFilePath}', ignoring");
            }
        }

        Composite.Priorities = priority;
    }

    public void SaveDefaultSheet()
    {
        Composite.SaveDefaultSheet($"{Path}/default.balancesheet");

        if (!File.Exists($"{Path}/user.balancesheet"))
        {
            BalanceSheet userSheet = null;

            if (!Composite.Sheets.ContainsKey("custom.user"))
            {
                userSheet = Composite.CreateSheet($"custom.user");
            }
            else
            {
                userSheet = Composite.Sheets["custom.user"];
            }


            userSheet.Save($"{Path}/user.balancesheet", Composite.EntryOrder, Composite.DefaultSheet.Values);
        }
    }

    public BalanceComposite Composite = null;
    public string Path = null;
}