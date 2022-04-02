using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public class DisableOnAwake : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}