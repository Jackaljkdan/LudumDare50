using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace JK.World
{
    [DisallowMultipleComponent]
    public class ColliderStayEvent : MonoBehaviour
    {
        #region Inspector

        public UnityEventCollision onCollisionStay = new UnityEventCollision();

        #endregion

        public Collider Collider { get; private set; }

        [Inject]
        private void Inject()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnCollisionStay(Collision collision)
        {
            onCollisionStay.Invoke(Collider, collision);
        }
    }
}