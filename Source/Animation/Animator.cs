using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public class AnimScriptAnimator : MonoBehaviour
{
    public IReadOnlyList<AnimatedTransform> Transforms => _transforms;
    [SerializeField] private List<AnimatedTransform> _transforms = new List<AnimatedTransform>();
    [SerializeField] private AnimationMixer _mixer = new AnimationMixer();

    public void ClearTransforms()
    {
        _transforms.Clear();
    }

    public void AddTransform(AnimatedTransform animated)
    {
        _transforms.Add(animated);

        _mixer.SetTrackDefaults(PositionTrackName(animated.Transform), Vector3.zero);
        _mixer.SetTrackDefaults(RotationTrackName(animated.Transform), Quaternion.identity);
        _mixer.SetTrackDefaults(ScaleTrackName(animated.Transform), Vector3.one);
    }

    public void RemoveTransform(Transform transform)
    {
        _transforms.RemoveAll((animated) => animated.Transform == transform);
    }

    public AnimationMixer.TrackOffset GetTrackOffset(string trackName, string offsetName)
    {
        return _mixer.GetTrackOffset(trackName, offsetName);
    }

    public void PlayAnimation(string name, Animation anim, float weight, AnimationEndMode endMode, float speed)
    {
        _mixer.PlayAnimation(name, anim, weight, endMode, speed);
    }

    public void StopAnimation(string name)
    {
        _mixer.StopAnimation(name);
    }

    protected void Awake()
    {
    }

    protected void Start()
    {
    }

    protected void LateUpdate()
    {
        _mixer.Advance(Time.deltaTime);

        foreach (var animated in _transforms)
        {
            if (animated == null)
            {
                continue;
            }

            if (animated.Transform == null)
            {
                continue;
            }

            Vector3 pos = animated.BaseLocalPosition;
            Quaternion rotation = animated.BaseLocalRotation;
            Vector3 scale = animated.BaseLocalScale;

            if (_mixer.TryGetValue<Vector3>(PositionTrackName(animated.Transform), out var animPos))
            {
                pos += animPos;
            }

            if (_mixer.TryGetValue<Quaternion>(RotationTrackName(animated.Transform), out var animRot))
            {
                rotation *= animRot;
            }

            if (_mixer.TryGetValue<Vector3>(ScaleTrackName(animated.Transform), out var animScale))
            {
                scale.Scale(animScale);
            }

            animated.Transform.localPosition = pos;
            animated.Transform.localRotation = rotation;
            animated.Transform.localScale = scale;
        }
    }

    public static string PositionTrackName(Transform tf)
    {
        return $"{tf.name}.position";
    }

    public static string RotationTrackName(Transform tf)
    {
        return $"{tf.name}.rotation";
    }

    public static string ScaleTrackName(Transform tf)
    {
        return $"{tf.name}.scale";
    }

    [Serializable]
    public class AnimatedTransform
    {
        public Transform Transform = null;

        public Vector3 BaseLocalPosition = Vector3.zero;
        public Quaternion BaseLocalRotation = Quaternion.identity;
        public Vector3 BaseLocalScale = Vector3.one;
    }
}