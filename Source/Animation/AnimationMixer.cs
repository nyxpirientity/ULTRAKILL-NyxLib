using System;
using System.Collections.Generic;
using System.IO;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public class AnimationMixer
{
    public bool TryGetValue<T>(string trackName, out T value)
    {
        if (!_currentFrames.TryGetValue(trackName, out var frame))
        {
            value = default;
            return false;
        }

        if (frame.Value is not T)
        {
            value = default;
            return false;
        }

        value = (T)frame.Value;
        return true;
    }

    public void SetTrackDefaults(string trackName, bool defaultValue)
    {
        SetTrackDefaults(trackName, KeyFrameType.Bool, defaultValue);
    }

    public void SetTrackDefaults(string trackName, float defaultValue)
    {
        SetTrackDefaults(trackName, KeyFrameType.Float, defaultValue);
    }

    public void SetTrackDefaults(string trackName, Vector2 defaultValue)
    {
        SetTrackDefaults(trackName, KeyFrameType.Vec2, defaultValue);
    }

    public void SetTrackDefaults(string trackName, Vector3 defaultValue)
    {
        SetTrackDefaults(trackName, KeyFrameType.Vec3, defaultValue);
    }

    public void SetTrackDefaults(string trackName, Vector4 defaultValue)
    {
        SetTrackDefaults(trackName, KeyFrameType.Vec4, defaultValue);
    }

    public void SetTrackDefaults(string trackName, Quaternion defaultValue)
    {
        SetTrackDefaults(trackName, KeyFrameType.Quat, defaultValue);
    }

    private void SetTrackDefaults(string trackName, KeyFrameType type, object value)
    {
        _trackDefaults.RemoveAll((td) => td.Name == trackName);
        _trackDefaults.Add(new TrackDefaults() { Value = value, Type = type, Name = trackName });
    }

    public AnimationPlayback PlayAnimation(string name, Animation anim, float weight, AnimationEndMode endMode = AnimationEndMode.OneShot, float speed = 1.0f)
    {
        StopAnimation(name);

        AnimationPlayback playback = new AnimationPlayback();
        playback.Animation = anim;
        playback.Time = 0.0f;
        playback.Speed = speed;
        playback.EndMode = endMode;

        _playbacks.Add(new MixedPlayback() { Playback = playback, Name = name, Weight = weight });

        return playback;
    }

    public void SetPlaybackWeight(string playbackName, float weight)
    {
        try
        {
            var playBack = _playbacks.Find((p) => p.Name == playbackName);
            playBack.Weight = weight;
        }
        catch (System.Exception)
        {
            PlayAnimation(playbackName, null, weight);
        }
    }

    public void SetPlaybackSpeed(string playbackName, float speed)
    {
        try
        {
            var playBack = _playbacks.Find((p) => p.Name == playbackName);
            playBack.Playback.Speed = speed;
        }
        catch (System.Exception)
        {
            PlayAnimation(playbackName, null, 1.0f, AnimationEndMode.OneShot, speed);
        }
    }

    public TrackOffset GetTrackOffset(string trackName, string offsetName)
    {
        var track = GetTrack(trackName);
        var offset = track.Offsets.Find((offset) => offset.Name == offsetName);

        if (offset != null)
        {
            return offset;
        }

        offset = new TrackOffset();
        offset.Name = offsetName;
        track.Offsets.Add(offset);

        return offset;
    }

    private MixedTrack GetTrack(string trackName)
    {
        var track = _tracks.Find((track) => track.Name == trackName);

        if (track != null)
        {
            return track;
        }

        track = new MixedTrack();
        track.Name = trackName;
        _tracks.Add(track);
        return track;
    }

    public void RemoveOffset(string trackName, string offsetName)
    {
        var track = GetTrack(trackName);
        track.Offsets.RemoveAll((a) => offsetName == a.Name);
    }

    public bool Advance(float time)
    {
        _currentFrames.Clear();

        float totalWeight = 0.0f;

        bool anyUnfinished = false;

        HashSet<int> finished = new HashSet<int>();

        for (int i = 0; i < _playbacks.Count; i++)
        {
            MixedPlayback mixed = _playbacks[i];

            if (!mixed.Playback.Advance(time))
            {
                anyUnfinished = true;
                totalWeight += mixed.Weight;
                continue;
            }

            finished.Add(i);
        }

        for (int i = 0; i < _playbacks.Count; i++)
        {
            var mixed = _playbacks[i];

            if (finished.Contains(i))
            {
                continue;
            }

            foreach (var track in mixed.Playback.Animation.Tracks)
            {
                if (!track.Value.TryGetFrameAt(mixed.Playback.Time, out var mixedFrame))
                {
                    continue;
                }

                AnimFrame pbFrame;

                if (!_currentFrames.ContainsKey(track.Key))
                {
                    pbFrame = new AnimFrame();
                    pbFrame.Time = mixed.Playback.Time;
                }
                else
                {
                    pbFrame = _currentFrames[track.Key];
                    if (pbFrame.Type != mixedFrame.Type)
                    {
                        Log.Warning($"AnimationMixer ignoring a track in an animation mixer due to data types not matching up between tracks of the same name '{track.Key}'");
                        continue;
                    }
                }

                pbFrame.Type = mixedFrame.Type;
                pbFrame.Value = VariantAnimMath.Compose(pbFrame.Type, pbFrame.Value, VariantAnimMath.Scale(mixedFrame.Type, mixedFrame.Value, mixed.Weight / totalWeight));

                _currentFrames[track.Key] = pbFrame;
            }
        }

        foreach (var def in _trackDefaults)
        {
            if (_currentFrames.ContainsKey(def.Name))
            {
                continue;
            }

            var frame = new AnimFrame();

            frame.Time = -1.0f;
            frame.Type = def.Type;
            frame.Value = def.Value;
            frame.Power = 1.0f;

            _currentFrames[def.Name] = frame;
        }

        foreach (var track in _tracks)
        {
            if (!_currentFrames.ContainsKey(track.Name))
            {
                continue;
            }

            track.ActiveFrame = _currentFrames[track.Name];

            foreach (var offset in track.Offsets)
            {
                track.ActiveFrame.Value = VariantAnimMath.Compose(track.ActiveFrame.Type, track.ActiveFrame.Value, offset.Offset);
            }

            _currentFrames[track.Name] = track.ActiveFrame;
        }

        return anyUnfinished;
    }

    public void StopAnimation(string name)
    {
        _playbacks.RemoveAll((mp) => mp.Name == name);
    }

    private Dictionary<string, AnimFrame> _currentFrames = new Dictionary<string, AnimFrame>();
    [SerializeField] private List<MixedPlayback> _playbacks = new List<MixedPlayback>();
    [SerializeField] private List<MixedTrack> _tracks = new List<MixedTrack>();
    [SerializeField] private List<TrackDefaults> _trackDefaults = new List<TrackDefaults>();

    [Serializable]
    struct MixedPlayback
    {
        public string Name;
        public AnimationPlayback Playback;
        public float Weight;
    }

    [Serializable]
    struct TrackDefaults
    {
        public string Name;
        [SerializeReference] public object Value;
        public KeyFrameType Type;
    }

    [Serializable]
    class MixedTrack
    {
        public string Name;
        public List<TrackOffset> Offsets = new List<TrackOffset>();

        public MixedTrack()
        {
        }

        public AnimFrame ActiveFrame;
    }

    [Serializable]
    public class TrackOffset
    {
        public TrackOffset() { }
        public string Name;
        public object Offset;
    }
}