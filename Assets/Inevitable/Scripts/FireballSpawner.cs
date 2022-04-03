using DG.Tweening;
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

        public float finalAverageSpawnSeconds = 1f;

        [SerializeField]
        private bool autoSpawn = true;

        public float targetNoise = 10;

        public float finalTargetNoise = 5;

        public Fireball fireballPrefab;

        public List<Building> specificTargets = new List<Building>();

        public Transform dummyTarget;

        #endregion

        [Inject(Id = "targets")]
        private List<Building> commonTargets = null;

        [Inject(Id = "difficulty.seconds")]
        private float secondsOverDifficultyIncrease = 0;

        private void Start()
        {
            if (autoSpawn)
            {
                DOTween.To(
                    () => averageSpawnSeconds,
                    val => averageSpawnSeconds = val,
                    finalAverageSpawnSeconds,
                    secondsOverDifficultyIncrease
                ).SetEase(Ease.Linear);

                DOTween.To(
                    () => targetNoise,
                    val => targetNoise = val,
                    finalTargetNoise,
                    secondsOverDifficultyIncrease
                ).SetEase(Ease.Linear);
            }
        }

        private void OnEnable()
        {
            if (autoSpawn)
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
            if (Spawn())
                ScheduleSpawn();
            else
                gameObject.SetActive(false);
        }

        public bool Spawn()
        {
            Fireball fireball = Instantiate(fireballPrefab);
            fireball.transform.position = transform.position;

            Transform targetTransform = dummyTarget;

            if (targetTransform == null)
            {
                Flammable target = FindTarget();

                if (target == null)
                    return false;

                targetTransform = target.transform;
            }

            Vector3 targetPosition = targetTransform.position;
            float noiseX = UnityEngine.Random.Range(-targetNoise, targetNoise);
            float noiseY = UnityEngine.Random.Range(-targetNoise, targetNoise);
            targetPosition.x += noiseX;
            targetPosition.y += noiseY;

            fireball.target = targetPosition;

            return true;
        }

        private Flammable FindTarget()
        {
            Building building = null;

            List<Building> targets = specificTargets.Count > 0 ? specificTargets : commonTargets;

            while (targets.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, targets.Count);
                building = targets[randomIndex];

                if (building.IsBurntDown)
                {
                    //Debug.Log("removing " + building.name);
                    targets.RemoveAt(randomIndex);
                }
                else
                {
                    break;
                }
            }

            if (building == null || building.IsBurntDown)
                return null;

            foreach (Flammable flammable in building.EnumerateFlammables())
                return flammable;

            return null;
        }
    }
}