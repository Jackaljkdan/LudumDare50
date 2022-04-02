using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace JK.World
{
    [RequireComponent(typeof(Collider))]
    public class ColliderUnfrequentEvents : MonoBehaviour
    {
        #region Inspector

        public UnityEventCollision onCollisionEnter = new UnityEventCollision();
        public UnityEventCollision onCollisionExit = new UnityEventCollision();

        #endregion

        public Collider Collider { get; private set; }

        [Inject]
        private void Inject()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter.Invoke(Collider, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            onCollisionExit.Invoke(Collider, collision);
        }
    }
}