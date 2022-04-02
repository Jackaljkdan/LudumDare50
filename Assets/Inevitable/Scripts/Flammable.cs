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

        public UnityEvent onBurnDown = new UnityEvent();

        [Header("Runtime")]
        public float normalizedBurningIntensity = 0;

        #endregion

        private float maxEmissionRate;

        private ParticleSystem.EmissionModule emission;

        private void Start()
        {
            maxEmissionRate = particles.emission.rateOverTime.constant;
        }

        [ContextMenu("Start burning")]
        public void StartBurning()
        {
            if (particles.isPlaying)
                return;

            emission = particles.emission;
            emission.rateOverTime = 0;
            normalizedBurningIntensity = Mathf.Epsilon;

            particles.Play();
        }

        [ContextMenu("Stop burning")]
        public void StopBurning()
        {
            if (!particles.isPlaying)
                return;

            particles.Stop();
        }

        private void Update()
        {
            if (!particles.isPlaying)
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
                onBurnDown.Invoke();
        }
    }
}