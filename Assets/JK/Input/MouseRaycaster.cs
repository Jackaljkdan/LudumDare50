using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Input
{
    public class MouseRaycaster : MonoBehaviour
    {
        #region Inspector fields

        public Mode mode = Mode.RaycastOnMousePressed;

        public int mouseButton = 0;

        public LayerMask layerMask = 0x7FFFFFFF;

        public float maxDistance = float.PositiveInfinity;

        public new Camera camera = null;

        public UnityEventMouseRaycasterHit onRaycastHit = new UnityEventMouseRaycasterHit();

        #endregion

        [Serializable]
        public enum Mode
        {
            RaycastOnMousePressed,
            RaycastEveryFrame,
        }

        private void Update()
        {
            if (camera != null && (mode == Mode.RaycastEveryFrame || UnityEngine.Input.GetMouseButtonDown(mouseButton)))
            {
                Ray ray = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
                    onRaycastHit?.Invoke(this, hit);
            }
        }
    }

    [Serializable]
    public class UnityEventMouseRaycaster : UnityEvent<MouseRaycaster> { }

    [Serializable]
    public class UnityEventMouseRaycasterHit : UnityEvent<MouseRaycaster, RaycastHit> { }
}