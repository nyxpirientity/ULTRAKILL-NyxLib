using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Assertions;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class StandardSerializerSet
{
    public static void AddStandardSerializers(this SerializerSet set)
    {
        set.Add<int>(
        serializer: (int val) =>
        {
            return val.ToString(CultureInfo.InvariantCulture).TrimEnd('0');
        },
        deserializer: (string data) =>
        {
            if (int.TryParse(data, System.Globalization.NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            throw new FormatException($"malformed integer format '{data}'");
        }, description: "integer");

        set.Add<float>(
        serializer: (float val) =>
        {
            return val.ToString(CultureInfo.InvariantCulture).TrimEnd('0');
        },
        deserializer: (string data) =>
        {
            if (float.TryParse(data, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            throw new FormatException($"malformed float format '{data}'");
        }, description: "decimal");

        set.Add<double>(
        serializer: (double val) =>
        {
            return val.ToString(CultureInfo.InvariantCulture).TrimEnd('0');
        },
        deserializer: (string data) =>
        {
            if (double.TryParse(data, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            throw new FormatException($"malformed double format '{data}'");
        }, description: "decimal");

        set.Add<bool>(
        serializer: (bool val) =>
        {
            if (val)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        },
        deserializer: (string data) =>
        {
            switch (data)
            {
                case "true":
                    return true;
                case "false":
                    return false;
            }

            throw new FormatException($"invalid boolean value: '{data}'");
        }, description: "binary true/false"
        );
    }
}