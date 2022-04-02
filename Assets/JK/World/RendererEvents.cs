using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.World
{
    [RequireComponent(typeof(Renderer))]
    public class RendererEvents : MonoBehaviour
    {
        #region Inspector

        public UnityEventRendererEvents onBecameVisible = new UnityEventRendererEvents();
        public UnityEventRendererEvents onBecameInvisible = new UnityEventRendererEvents();

        #endregion

        private void OnBecameVisible()
        {
            onBecameVisible.Invoke(this);
        }

        private void OnBecameInvisible()
        {
            onBecameInvisible.Invoke(this);
        }
    }
    
    [Serializable]
    public class UnityEventRendererEvents : UnityEvent<RendererEvents> { }
}