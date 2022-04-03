using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class Flammable : MonoBehaviour
    {
        #region Inspector

        public ParticleSystem particles;

        public float burnSeconds = 5;

        public UnityEvent onStartBurning = new UnityEvent();
        public UnityEvent onBurnDown = new UnityEvent();

        [Header("Runtime")]
        public float normalizedBurningIntensity = 0;

        [Header("Internals")]
        public Transform model = null;

        #endregion

        public const float DestroyDelaySeconds = 4;

        public bool IsBurning => particles.isPlaying;

        public bool IsBurntDown { get; private set; }

        public bool CanStartBurning => !IsBurntDown && !IsBurning;

        private float maxEmissionRate;

        private ParticleSystem.EmissionModule emission;

        private void Start()
        {
            maxEmissionRate = particles.emission.rateOverTime.constant;
        }

        [ContextMenu("Start burning")]
        public void StartBurning()
        {
            if (particles.isPlaying || IsBurntDown)
                return;

            emission = particles.emission;
            emission.rateOverTime = 0;
            normalizedBurningIntensity = Mathf.Epsilon;

            particles.Play();

            onStartBurning.Invoke();
        }

        [ContextMenu("Stop burning")]
        public void StopBurning()
        {
            if (!particles.isPlaying || IsBurntDown)
                return;

            particles.Stop();
        }

        public void BurnDown()
        {
            IsBurntDown = true;
            emission.rateOverTime = maxEmissionRate * 1.5f;

            model.DOScale(0, 0.5f).onComplete += () =>
            {
                particles.Stop();
                Invoke(nameof(Destroy), DestroyDelaySeconds);
            };

            onBurnDown.Invoke();
        }

        private void Update()
        {
            if (!particles.isPlaying || IsBurntDown)
                return;

            if (normalizedBurningIntensity <= 0)
            {
                StopBurning();
                return;
            }

            if (normalizedBurningIntensity >= 1)
                return;

            float bump = Time.deltaTime / burnSeconds;
            normalizedBurningIntensity += bump;

            normalizedBurningIntensity = Mathf.Clamp01(normalizedBurningIntensity);

            emission.rateOverTime = normalizedBurningIntensity * maxEmissionRate;

            if (normalizedBurningIntensity == 1)
                BurnDown();
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}