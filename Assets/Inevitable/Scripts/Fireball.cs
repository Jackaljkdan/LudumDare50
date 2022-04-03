using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class Fireball : MonoBehaviour
    {
        #region Inspector

        public Vector3 target;

        public float speed = 1;

        public Transform explosionPrefab;

        public List<AudioClip> boomClips;

        #endregion

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            Vector3 direction = (target - transform.position).normalized;
            body.velocity = direction * speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Transform explosion = Instantiate(explosionPrefab);
            explosion.position = transform.position;

            int randomIndex = UnityEngine.Random.Range(0, boomClips.Count);
            AudioClip randomClip = boomClips[randomIndex];
            
            var source = GetComponent<AudioSource>();
            source.Stop();
            source.PlayOneShot(randomClip);

            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());

            foreach (Transform child in transform)
                child.gameObject.SetActive(false);

            Invoke(nameof(Destroy), randomClip.length + 1);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}