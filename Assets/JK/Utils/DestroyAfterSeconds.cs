using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class DestroyAfterSeconds : MonoBehaviour
    {
        #region Inspector

        public float seconds = 1;

        #endregion

        private void Start()
        {
            Invoke(nameof(DoIt), seconds);
        }

        private void DoIt()
        {
            Destroy(gameObject);
        }
    }
}