using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public struct AnimFrame
{
    public KeyFrameType Type;
    public object Value;
    public float Time;
    public float Power = 1.0f;

    public AnimFrame()
    {
    }
}