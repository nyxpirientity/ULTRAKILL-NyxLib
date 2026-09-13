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
        Assert.IsNotNull(Composite);

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

        foreach (var filePath in files)
        {
            if (!filePath.EndsWith(".balancesheet"))
            {
                continue;
            }

            string fileName = filePath.Substring(filePath.LastIndexOf('.') - 1, filePath.Length).Substring(filePath.LastIndexOfAny(['/', '\\']));

            if (string.IsNullOrWhiteSpace(fileName))
            {
                continue;
            }

            string data = File.ReadAllText(filePath);
            string sheetId = $"custom.{fileName}";

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
        }
    }

    public BalanceComposite Composite = null;
    public string Path = null;
}