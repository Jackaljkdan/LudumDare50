using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inevitable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class FlammableCollider : MonoBehaviour
    {
        #region Inspector

        public Flammable target;

        public UnityEvent onHit = new UnityEvent();

        private void Reset()
        {
            target = GetComponentInParent<Flammable>();
        }

        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Fireball"))
            {
                target.StartBurning();
                onHit.Invoke();
            }
        }
    }
}