using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class ManSpawner : MonoBehaviour
    {
        #region Inspector

        public Transform spawnAnchor;

        public Man manPrefab;

        public float minSecondsBetweenSpawns = 5;

        public int maxSpawnedTogether = 4;

        #endregion

        [Inject]
        private DiContainer diContainer = null;

        private float lastSpawnTime;

        private void Start()
        {
            foreach (var fc in GetComponentsInChildren<FlammableCollider>())
                fc.onHit.AddListener(OnHit);
        }

        private void OnHit()
        {
            if (Time.time - lastSpawnTime > minSecondsBetweenSpawns)
                Spawn();
        }

        public void Spawn()
        {
            int count = UnityEngine.Random.Range(1, maxSpawnedTogether + 1);

            for (int i = 0; i < count; i++)
            {
                Man man = Instantiate(manPrefab);
                diContainer.InjectGameObject(man.gameObject);

                man.transform.position = spawnAnchor.position;
                man.transform.forward = spawnAnchor.forward;
            }

            lastSpawnTime = Time.time;
        }
    }
}