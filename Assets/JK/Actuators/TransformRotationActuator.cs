using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class TransformRotationActuator : MonoBehaviour, IRotationActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 40;

        public float lowerBound = 360 - 89;
        public float upperBound = 89;

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Vector2 Input { get; set; }

        private void Update()
        {
            if (Input.sqrMagnitude == 0)
                return;

            Rotate(transform, Input * Speed * Time.deltaTime, lowerBound, upperBound);
        }

        public static void Rotate(Transform transform, Vector2 degrees, float lowerBound = 360 - 89, float upperBound = 89)
        {
            transform.RotateAround(transform.position, Vector3.up, degrees.x);

            Vector3 euler = transform.localEulerAngles;
            euler.x -= degrees.y;

            if (euler.x > 180)
                euler.x = Mathf.Max(lowerBound, euler.x);
            else
                euler.x = Mathf.Min(upperBound, euler.x);

            transform.localEulerAngles = euler;

            //Debug.Log("euler " + transform.eulerAngles);
        }
    }

    [Serializable]
    public class UnityEventTransformRotationActuator : UnityEvent<TransformRotationActuator> { }
}