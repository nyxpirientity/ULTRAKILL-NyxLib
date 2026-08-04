using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public struct VelocityTracker
    {
        public VelocityTracker(Vector3 position, Quaternion rotation)
        {
            _linearVelocity = Vector3.zero;
            _angularVelocity = Vector3.zero;
            _LastTickPosition = position;
            _LastTickRotation = rotation;
        }

        public void Update(float delta, Vector3 position, Quaternion rotation)
        {
            _linearVelocity = (position - _LastTickPosition) / delta;

            Quaternion angularDiff = Quaternion.Inverse(_LastTickRotation) * rotation;
            angularDiff.ToAngleAxis(out float angle, out _angularVelocity);
            _angularVelocity *= Quaternion.Angle(_LastTickRotation, rotation) / delta;

            _LastTickPosition = position;
            _LastTickRotation = rotation;
        }

        public Vector3 LinearVelocity => _linearVelocity;
        public Vector3 AngularVelocity => _angularVelocity;

        private Vector3 _linearVelocity;
        private Vector3 _angularVelocity;

        private Vector3 _LastTickPosition;
        private Quaternion _LastTickRotation;
    }
}