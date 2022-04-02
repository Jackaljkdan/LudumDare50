using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inevitable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class Fireball : MonoBehaviour
    {
        #region Inspector

        public Vector3 target;

        public float speed = 1;

        public Transform explosionPrefab;

        #endregion

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            Vector3 direction = (target - transform.position).normalized;
            body.velocity = direction * speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
            Transform explosion = Instantiate(explosionPrefab);
            explosion.position = transform.position;
        }
    }
}