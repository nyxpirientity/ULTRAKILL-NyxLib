using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public class Animation : ScriptableObject
{
    public IReadOnlyDictionary<string, AnimationTrack> Tracks => _tracks;

    public float TotalDuration { get; private set; } = 0.0f;

    public void ParseScript(string data)
    {
        Clear();

        StringReader stringReader = new(data);
        long lineNum = -1;

        for (string line = stringReader.ReadLine(); line != null; line = stringReader.ReadLine())
        {
            lineNum += 1;
            line = line.TrimStart([' ', '\t']);

            if (line.StartsWith('#'))
            {
                continue;
            }

            var cmd = new Command(line);

            if (cmd.Parameters.Count == 0)
            {
                continue;
            }

            try
            {
                switch (cmd.Parameters[0])
                {
                    case "new-track":
                        NewTrackCommand(cmd);
                        break;
                    case "key":
                        KeyCommand(cmd);
                        break;
                    case "rotate-key":
                        RotateKeyCommand(cmd);
                        break;
                    case "add-to-key":
                        AddToKeyCommand(cmd);
                        break;
                    case "global-speed":
                        GlobalSpeedCommand(cmd);
                        break;
                }
            }
            catch (System.FormatException e)
            {
                throw new FormatException($"Failed to parse animation script\n Failed Line {lineNum}: {line}\nException: {e}");
            }
        }

        SolveDuration();
    }

    private void GlobalSpeedCommand(Command cmd)
    {
        cmd.HaveNumParamsOrThrow(2, new FormatException("global-speed command requires a parameter to represent the speed scalar, as a float"));

        if (!TryParseFloat(cmd.Parameters[1], out float speed))
        {
            new FormatException("global-speed command speed value float parse failed");
        }

        GlobalSpeed = speed;
    }

    private void SolveDuration()
    {
        float longestDuration = 0.0f;

        foreach (var track in _tracks.Values)
        {
            if (track.KeyFrames.Count == 0)
            {
                continue;
            }

            track.Duration = track.KeyFrames.Last().Time;

            longestDuration = Mathf.Max(longestDuration, track.Duration);
        }

        TotalDuration = longestDuration;
    }

    public void Clear()
    {
        ClearTracks();
        GlobalSpeed = 1.0f;
    }

    private void ClearTracks()
    {
        _tracks.Clear();
    }

    private void AddToKeyCommand(Command cmd)
    {
        cmd.HaveChainOrThrow("/track:1", new FormatException($"add to key command requires a track to identify the key"));
        cmd.HaveChainOrThrow("/additive:1", new FormatException($"add to key command requires a scope named additive to be added to the key"));

        var trackName = cmd.Scopes["track"].Parameters[0];

        if (!Tracks.ContainsKey(trackName))
        {
            throw new FormatException($"Track '{trackName}' doesn't seem to exist yet");
        }

        var track = Tracks[trackName];
        var additiveScope = cmd.Scopes["additive"];

        if (track.KeyFrames.Count == 0)
        {
            throw new FormatException($"Track '{trackName}' existed, but seemingly had no keyframes");
        }

        var keyFrame = track.KeyFrames.Last();

        switch (track.KeyFrameType)
        {
            case KeyFrameType.Bool:
                if (!bool.TryParse(additiveScope.Parameters[0], out var boolB))
                {
                    throw new FormatException($"failed to parse additive '{cmd.Scopes["additive"].Parameters[0]}' as a bool");
                }

                bool boolA = (bool)keyFrame.Value;
                keyFrame.Value = VariantAnimMath.BoolCompose(boolA, boolB);

                break;
            case KeyFrameType.Float:
                if (!TryParseFloat(additiveScope.Parameters[0], out var floatB))
                {
                    throw new FormatException($"failed to parse additive '{cmd.Scopes["additive"].Parameters[0]}' as a float");
                }

                float floatA = (float)keyFrame.Value;
                keyFrame.Value = floatA + floatB;

                break;
            case KeyFrameType.Vec2:
                Vector2 vec2A = (Vector2)keyFrame.Value;
                Vector2 vec2B = ParseVec2FromCmdScope(additiveScope);
                keyFrame.Value = vec2A + vec2B;
                break;
            case KeyFrameType.Vec3:
                Vector3 vec3A = (Vector3)keyFrame.Value;
                Vector3 vec3B = ParseVec3FromCmdScope(additiveScope);
                keyFrame.Value = vec3A + vec3B;
                break;
            case KeyFrameType.Vec4:
                Vector4 vec4A = (Vector4)keyFrame.Value;
                Vector4 vec4B = ParseVec4FromCmdScope(additiveScope);
                keyFrame.Value = vec4A + vec4B;
                break;
            case KeyFrameType.Quat:
                Quaternion quatA = (Quaternion)keyFrame.Value;
                Quaternion quatB = ParseQuatFromCmdScope(additiveScope);
                keyFrame.Value = quatA * quatB;
                break;
        }

        track.KeyFrames[track.KeyFrames.Count - 1] = keyFrame;
    }

    private void RotateKeyCommand(Command cmd)
    {
        cmd.HaveChainOrThrow("/track:1", new FormatException("rotate key command requires a track scope with 1 parameter representing the track name"));
        cmd.HaveChainOrThrow("/quat:3", new FormatException("rotate key command requires a quat scope with 3-4 parameters depending on the quaternion mode used, as the quat to rotate the value with"));

        var trackName = cmd.Scopes["track"].Parameters[0];

        if (!Tracks.TryGetValue(trackName, out var track))
        {
            throw new FormatException($"track '{trackName}' didn't seem to exist");
        }

        if (track.KeyFrameType != KeyFrameType.Vec3)
        {
            throw new FormatException($"{TrackTypeMethods.TrackTypeToScriptString(track.KeyFrameType)} not supported for rotate key, currently only vec3 is supported");
        }

        if (track.KeyFrames.Count == 0)
        {
            throw new FormatException($"'{trackName}' didn't seem to have keyframes yet");
        }

        Vector3 axis = Vector3.zero;

        if (cmd.HasScope("axis"))
        {
            var axisScope = cmd.Scopes["axis"];
            axis = ParseVec3FromCmdScope(axisScope);
        }

        var quat = ParseQuatFromCmdScope(cmd.Scopes["quat"]);

        var keyFrame = track.KeyFrames.Last();

        var vec = (Vector3)keyFrame.Value;

        vec = (quat * (vec - axis)) + axis;

        keyFrame.Value = vec;

        track.KeyFrames[track.KeyFrames.Count - 1] = keyFrame;
    }

    private void NewTrackCommand(Command cmd)
    {
        cmd.HaveChainOrThrow("/name:1", new FormatException($"name scope required with 1 parameter to represent the name"));
        cmd.HaveChainOrThrow("/type:1", new FormatException($"type scope required with 1 parameter to represent the type"));

        var name = cmd.Scopes["name"].Parameters[0];
        var typeStr = cmd.Scopes["type"].Parameters[0];

        var maybeType = TrackTypeMethods.StringToTrackType(typeStr);

        if (!maybeType.HasValue)
        {
            throw new FormatException($"invalid track type '{typeStr}");
        }

        var type = maybeType.Value;

        switch (type)
        {
            case KeyFrameType.Bool:
                CreateBoolTrack(name);
                break;
            case KeyFrameType.Float:
                CreateFloatTrack(name);
                break;
            case KeyFrameType.Vec2:
                CreateVec2Track(name);
                break;
            case KeyFrameType.Vec3:
                CreateVec3Track(name);
                break;
            case KeyFrameType.Vec4:
                CreateVec4Track(name);
                break;
            case KeyFrameType.Quat:
                CreateQuatTrack(name);
                break;
        }
    }

    public AnimationTrack CreateBoolTrack(string trackName) => CreateTrack(trackName, KeyFrameType.Bool);
    public AnimationTrack CreateFloatTrack(string trackName) => CreateTrack(trackName, KeyFrameType.Float);
    public AnimationTrack CreateVec2Track(string trackName) => CreateTrack(trackName, KeyFrameType.Vec2);
    public AnimationTrack CreateVec3Track(string trackName) => CreateTrack(trackName, KeyFrameType.Vec3);
    public AnimationTrack CreateVec4Track(string trackName) => CreateTrack(trackName, KeyFrameType.Vec4);
    public AnimationTrack CreateQuatTrack(string trackName) => CreateTrack(trackName, KeyFrameType.Quat);

    private AnimationTrack CreateTrack(string trackName, KeyFrameType type)
    {
        var trackData = new AnimationTrack();
        trackData.KeyFrameType = type;
        _tracks.Add(trackName, trackData);
        return trackData;
    }

    private Dictionary<string, AnimationTrack> _tracks = new Dictionary<string, AnimationTrack>();
    public float GlobalSpeed = 1.0f;

    private void KeyCommand(Command cmd)
    {
        cmd.HaveChainOrThrow("/track:1", new FormatException($"key: key command requires track scope with 1 param to represent track name"));
        cmd.HaveChainOrThrow("/target:1", new FormatException($"key: key command requires target scope with at least 1 param to represent the target value, some track types require more than 1"));
        cmd.HaveChainOrThrow("/duration:1", new FormatException($"key: key command requires duration scope with 1 param to represent the duration"));

        var trackScope = cmd.Scopes["track"];
        var durationScope = cmd.Scopes["duration"];
        var targetScope = cmd.Scopes["target"];

        var trackName = trackScope.Parameters[0];

        if (!float.TryParse(durationScope.Parameters[0], out var duration))
        {
            throw new FormatException($"key: duration scope did not seem to have a valid float (decimal/number) value");
        }

        duration /= Mathf.Max(GlobalSpeed, 0.001f);

        if (!Tracks.ContainsKey(trackName))
        {
            throw new FormatException($"Track not yet created, {trackName}");
        }

        var track = Tracks[trackName];
        object kfValue;

        if (track.KeyFrameType is KeyFrameType.Bool)
        {
            bool value;

            if (!bool.TryParse(targetScope.Parameters[0], out value))
            {
                throw new FormatException($"invalid bool formatting in target bool");
            }

            kfValue = value;
        }
        else if (track.KeyFrameType is KeyFrameType.Float)
        {
            float value;

            if (!TryParseFloat(targetScope.Parameters[0], out value))
            {
                throw new FormatException($"invalid float formatting in target float");
            }

            kfValue = value;
        }
        else if (track.KeyFrameType is KeyFrameType.Vec2)
        {
            Vector2 value = ParseVec2FromCmdScope(targetScope);

            kfValue = value;
        }
        else if (track.KeyFrameType is KeyFrameType.Vec3)
        {
            Vector3 value = ParseVec3FromCmdScope(targetScope);

            kfValue = value;
        }
        else if (track.KeyFrameType is KeyFrameType.Vec4)
        {
            Vector4 value = ParseVec4FromCmdScope(targetScope);

            kfValue = value;
        }
        else if (track.KeyFrameType is KeyFrameType.Quat)
        {
            Quaternion value = ParseQuatFromCmdScope(targetScope);
            kfValue = value;
        }
        else
        {
            throw new FormatException($"odd error, unknown TrackType");
        }

        float power = 1.0f;

        if (cmd.HasScope("power"))
        {
            if (cmd.Scopes["power"].HasNumParams(1))
            {
                if (!TryParseFloat(cmd.Scopes["power"].Parameters[0], out power))
                {
                    power = 1.0f;
                }
            }
        }

        track.KeyFrames.Add(new AnimFrame() { Value = kfValue, Time = track.Duration + duration, Type = track.KeyFrameType, Power = power });
        track.Duration += duration;
    }

    private Vector4 ParseVec4FromCmdScope(CommandScope targetScope)
    {
        if (targetScope.Parameters.Count < 4)
        {
            throw new FormatException($"vec4 track expected 4 parameters");
        }

        Vector4 value;

        if (!TryParseFloat(targetScope.Parameters[0], out value.x) ||
            !TryParseFloat(targetScope.Parameters[1], out value.y) ||
            !TryParseFloat(targetScope.Parameters[2], out value.z) ||
            !TryParseFloat(targetScope.Parameters[3], out value.w))
        {
            throw new FormatException($"invalid float formatting in target vec4");
        }

        return value;
    }

    private Vector3 ParseVec3FromCmdScope(CommandScope targetScope)
    {
        if (targetScope.Parameters.Count < 3)
        {
            throw new FormatException($"vec3 track expected 3 parameters");
        }

        Vector3 value;

        if (!TryParseFloat(targetScope.Parameters[0], out value.x) ||
            !TryParseFloat(targetScope.Parameters[1], out value.y) ||
            !TryParseFloat(targetScope.Parameters[2], out value.z))
        {
            throw new FormatException($"invalid float formatting in target vec3");
        }

        return value;
    }

    private Vector2 ParseVec2FromCmdScope(CommandScope targetScope)
    {
        if (targetScope.Parameters.Count < 2)
        {
            throw new FormatException($"vec2 track expected 2 parameters");
        }

        Vector2 value;

        if (!TryParseFloat(targetScope.Parameters[0], out value.x) ||
            !TryParseFloat(targetScope.Parameters[1], out value.y))
        {
            throw new FormatException($"invalid float formatting in target vec2");
        }

        return value;
    }

    private Quaternion ParseQuatFromCmdScope(CommandScope targetScope)
    {
        Quaternion value;
        if (targetScope.HasFlag('r'))
        {
            if (targetScope.Parameters.Count < 4)
            {
                throw new FormatException($"quat tracks with -r flag are intepreted as raw quaternion values, which expects 4 parameters to represent x, y, z, and w values");
            }

            if (!TryParseFloat(targetScope.Parameters[0], out value.x) ||
                !TryParseFloat(targetScope.Parameters[1], out value.y) ||
                !TryParseFloat(targetScope.Parameters[2], out value.z) ||
                !TryParseFloat(targetScope.Parameters[3], out value.w))
            {
                throw new FormatException($"invalid float formatting in target raw quat");
            }
        }
        else
        {
            if (targetScope.Parameters.Count < 3)
            {
                throw new FormatException($"quat tracks are interpreted as euler rotations by default (unless -r flag is passed into target scope), which expects 3 parameters to represent pitch, yaw, and roll");
            }

            Vector3 vec;

            if (!TryParseFloat(targetScope.Parameters[0], out vec.x) ||
                !TryParseFloat(targetScope.Parameters[1], out vec.y) ||
                !TryParseFloat(targetScope.Parameters[2], out vec.z))
            {
                throw new FormatException($"invalid float formatting in target euler quat");
            }

            value = Quaternion.Euler(vec);
        }

        return value;
    }

    private bool TryParseFloat(string str, out float value) => float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
}