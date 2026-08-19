using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

[Serializable]
public class AnimationTrack
{
    public KeyFrameType KeyFrameType = KeyFrameType.Bool;
    public List<AnimFrame> KeyFrames = new List<AnimFrame>();
    public float Duration = 0.0f;

    public bool TryGetFrameAt(float time, out AnimFrame frame)
    {
        if (KeyFrames.Count == 0)
        {
            frame = new AnimFrame() { Type = KeyFrameType, Time = -1.0f, Value = null, Power = 0.0f };
            return false;
        }

        time = Mathf.Clamp(time, 0.0f, Duration - 0.001f);

        AnimFrame? keyFrameA = null;
        AnimFrame keyFrameB = KeyFrames.Last();

        for (int i = 0; i < KeyFrames.Count; i++)
        {
            if (KeyFrames[i].Time > time)
            {
                keyFrameB = KeyFrames[i];
                break;
            }
            else
            {
                keyFrameA = KeyFrames[i];
            }
        }

        frame = default;

        var keyFrameAVal = keyFrameA.HasValue ? keyFrameA.Value.Value : null;
        var keyFrameATime = keyFrameA.HasValue ? keyFrameA.Value.Time : 0.0f;
        var alpha = NyxMath.NormalizeToRange(time, keyFrameATime, keyFrameB.Time);
        var keyFrameAPower = keyFrameA.HasValue ? keyFrameA.Value.Power : 1.0f;

        frame.Value = VariantAnimMath.Lerp(KeyFrameType, keyFrameAVal, keyFrameB.Value, alpha, keyFrameB.Power);
        frame.Time = time;
        frame.Type = KeyFrameType;

        return true;
    }
}