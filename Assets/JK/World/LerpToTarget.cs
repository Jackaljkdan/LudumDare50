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

        public float lerp = 0.1f;

        #endregion

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target, lerp);
        }
    }
}