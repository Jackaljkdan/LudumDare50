using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class FireballSpawner : MonoBehaviour
    {
        #region Inspector

        public float averageSpawnSeconds = 5;

        public float targetNoise = 10;

        public Fireball fireballPrefab;

        #endregion

        [Inject(Id = "targets")]
        private List<Building> targets = null;

        private void OnEnable()
        {
            ScheduleSpawn();
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(SpawnAndSchedule));
        }

        private void ScheduleSpawn()
        {
            Invoke(nameof(SpawnAndSchedule), MathUtils.RandomExponentialInversed(averageSpawnSeconds));
        }

        private void SpawnAndSchedule()
        {
            Spawn();
            ScheduleSpawn();
        }

        private void Spawn()
        {
            Fireball fireball = Instantiate(fireballPrefab);
            fireball.transform.position = transform.position;

            int randomIndex = UnityEngine.Random.Range(0, targets.Count);
            var target = targets[randomIndex];

            Vector3 targetPosition = target.transform.position;
            float noiseX = UnityEngine.Random.Range(-targetNoise, targetNoise);
            float noiseY = UnityEngine.Random.Range(-targetNoise, targetNoise);
            targetPosition.x += noiseX;
            targetPosition.y += noiseY;

            fireball.target = targetPosition;
        }
    }
}