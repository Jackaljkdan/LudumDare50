using System;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Dev
{
    public class OrientationGizmo : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private bool drawOnlyWhenSelected = false;

        [SerializeField]
        private float length = 1;

        #endregion

        private void OnDrawGizmos()
        {
            if (drawOnlyWhenSelected)
                return;
            
            Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawOnlyWhenSelected)
                return;

            Draw();
        }

        private void Draw()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * length);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * length);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * length);
        }

    }
    
    [Serializable]
    public class UnityEventOrientationGizmo : UnityEvent<OrientationGizmo> { }
}