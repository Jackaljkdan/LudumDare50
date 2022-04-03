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
    public class FlamesSpreader : MonoBehaviour
    {
        #region Inspector

        public float intensityThreshold = 0.5f;

        public float _distanceThreshold = 1;

        public float secondsBetweenSpreads = 2f;

        public float finalSecondsBetweenSpreads = 0.5f;

        [SerializeField]
        private Flammable flammable = null;

        private void Reset()
        {
            flammable = GetComponent<Flammable>();
        }

        private void OnValidate()
        {
            cachedSibilings = null;
        }

        #endregion

        //public float DistanceThreshold
        //{
        //    get => _distanceThreshold;
        //    set
        //    {
        //        if (_distanceThreshold != value)
        //        {
        //            _distanceThreshold = value;
        //            cachedSibilings = null;
        //        }
        //    }
        //}

        public bool CanSpread
        {
            get => flammable.normalizedBurningIntensity >= intensityThreshold && Time.time - lastSpreadTime > secondsBetweenSpreads;
        }

        //private float previousIntensity = 0;

        private float lastSpreadTime;

        private List<Flammable> cachedSibilings;

        [Inject(Id = "difficulty.seconds")]
        private float secondsOverDifficultyIncrease = 0;

        private void Start()
        {
            flammable.onBurnDown.AddListener(OnBurnDown);

            DOTween.To(
                () => secondsBetweenSpreads,
                val => secondsBetweenSpreads = val,
                finalSecondsBetweenSpreads,
                secondsOverDifficultyIncrease
            ).SetEase(Ease.Linear);
        }

        private void OnBurnDown()
        {
            enabled = false;
        }

        private void LateUpdate()
        {
            if (!flammable.IsBurning)
                return;

            if (CanSpread)
                SpreadFlames();

            //previousIntensity = flammable.normalizedBurningIntensity;
        }

        [ContextMenu("Spread flames")]
        public void SpreadFlames()
        {
            lastSpreadTime = Time.time;

            if (cachedSibilings == null)
            {
                cachedSibilings = new List<Flammable>();

                //float sqrDistanceThreshold = DistanceThreshold * DistanceThreshold;
                Transform tr = transform;

                foreach (Transform child in transform.parent)
                {
                    if (child == tr)
                        continue;

                    if (!(child.TryGetComponent(out Flammable sibiling)))
                        continue;

                    //float sqrDistance = (tr.position - child.position).sqrMagnitude;

                    //if (sqrDistance <= sqrDistanceThreshold)
                    cachedSibilings.Add(sibiling);
                }

                ListUtils.ShuffleInPlace(cachedSibilings);
            }

            foreach (var sibiling in cachedSibilings)
            {
                if (sibiling == null)
                    continue;

                if (!sibiling.CanStartBurning)
                    continue;

                //Debug.Log("spreading to " + sibiling.name);

                sibiling.StartBurning();
                break;
            }
        }
    }
}