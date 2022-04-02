using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class InertialTransformRotationActuator : MonoBehaviour, IRotationActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 40;

        [SerializeField]
        private float lerp = 0.3f;

        public float lowerBound = 360 - 89;
        public float upperBound = 89;

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Vector2 Input { get; set; }

        private Vector3 inertia;

        private void Update()
        {
            inertia = Vector3.Lerp(inertia, Input, lerp);

            if (Mathf.Approximately(inertia.sqrMagnitude, 0))
                return;

            TransformRotationActuator.Rotate(transform, inertia * Speed * Time.deltaTime, lowerBound, upperBound);
        }
    }
    
    [Serializable]
    public class UnityEventInertialTransformRotationActuator : UnityEvent<InertialTransformRotationActuator> { }
}