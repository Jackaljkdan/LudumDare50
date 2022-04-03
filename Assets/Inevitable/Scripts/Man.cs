using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class Man : MonoBehaviour
    {
        #region Inspector

        public float maxAngle = 90;

        public float minSpeed = 3;
        public float maxSpeed = 6;

        public float minSeconds = 2;
        public float maxSeconds = 100;

        public float destroyObjectSqrDistanceThreshold = 100 * 100;

        public float destroySecondsAfterDeath = 30;

        [Header("Runtime")]

        [SerializeField]
        private float angle;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float seconds;
        [SerializeField]
        private float elapsed;

        #endregion

        [Inject(Id = "camera")]
        private Transform cameraTransform = null;

        private void Start()
        {
            angle = UnityEngine.Random.Range(-maxAngle, maxAngle);
            speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
            seconds = UnityEngine.Random.Range(minSeconds, maxSeconds);
        }

        private void Update()
        {
            elapsed += Time.deltaTime;

            Transform tr = transform;

            if (elapsed < seconds)
            {
                tr.position += tr.TransformDirection(Vector3.forward) * Time.deltaTime * speed;

                if ((cameraTransform.position - tr.position).sqrMagnitude >= destroyObjectSqrDistanceThreshold)
                {
                    Destroy(gameObject);
                    return;
                }

                tr.Rotate(Vector3.up, Time.deltaTime * angle / seconds);
            }
            else
            {
                enabled = false;
                tr.DORotate(Quaternion.LookRotation(Vector3.down, tr.TransformDirection(Vector3.forward)).eulerAngles, 0.25f);
                Invoke(nameof(Destroy), destroySecondsAfterDeath);
            }
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}