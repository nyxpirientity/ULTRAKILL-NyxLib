using System;
using System.IO;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public enum AnimationEndMode
{
    OneShot, Loop
}

[Serializable]
public class AnimationPlayback
{
    [SerializeReference] public Animation Animation = null;

    public bool TryGetFrameForTrack(string trackName, out AnimFrame frame)
    {
        if (Animation == null)
        {
            frame = default;
            return false;
        }

        if (!Animation.Tracks.TryGetValue(trackName, out var track))
        {
            frame = default;
            return false;
        }

        return track.TryGetFrameAt(Time, out frame);
    }

    public AnimationEndMode EndMode = AnimationEndMode.OneShot;

    public bool Advance(float time)
    {
        if (Animation == null)
        {
            return true;
        }

        time *= Speed;

        float rawNextTime = Time + time;

        Time = rawNextTime;

        if (rawNextTime > Animation.TotalDuration || rawNextTime < 0.0f)
        {
            return true;
        }

        return false;
    }

    public float Speed = 1.0f;

    public float Time
    {
        get => _time;
        set
        {
            switch (EndMode)
            {
                case AnimationEndMode.OneShot:
                    _time = Mathf.Clamp(value, 0.0f, Animation.TotalDuration);
                    break;
                case AnimationEndMode.Loop:
                    _time = value % Animation.TotalDuration;
                    break;
            }
        }
    }

    [SerializeField] private float _time = 0.0f;
}