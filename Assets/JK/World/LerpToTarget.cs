using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.World
{
    [DisallowMultipleComponent]
    public class LerpToTarget : MonoBehaviour
    {
        #region Inspector

        public Vector3 target;

        public float maxLerp = 0.6f;
        public float minLerp = 0.05f;

        public float sqrMaxDistance = 25;

        #endregion

        private void LateUpdate()
        {
            Transform tr = transform;

            float sqrDistance = (tr.position - target).sqrMagnitude;
            float lerp = Mathf.Lerp(minLerp, maxLerp, sqrDistance / sqrMaxDistance);

            tr.position = Vector3.Lerp(tr.position, target, lerp);
        }
    }
}