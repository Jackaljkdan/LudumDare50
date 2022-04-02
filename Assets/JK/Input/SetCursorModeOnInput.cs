using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Input
{
    [DisallowMultipleComponent]
    public class SetCursorModeOnInput : MonoBehaviour
    {
        #region Inspector

        public CursorLockMode mode = CursorLockMode.Locked;

        public DestroyMode destroyMode = DestroyMode.DestroyGameObject;

        public UnityEvent onInput = new UnityEvent();

        #endregion

        [Serializable]
        public enum DestroyMode
        {
            None,
            DestroySelf,
            DestroyGameObject,
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                Cursor.lockState = mode;

                switch (destroyMode)
                {
                    case DestroyMode.DestroySelf:
                        Destroy(this);
                        break;
                    case DestroyMode.DestroyGameObject:
                        Destroy(gameObject);
                        break;
                }

                onInput?.Invoke();
            }
        }
    }
}