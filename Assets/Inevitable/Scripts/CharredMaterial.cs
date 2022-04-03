using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class CharredMaterial : MonoBehaviour
    {
        #region Inspector

        public Flammable flammable;

        public Renderer target;

        public Color charredColor = Color.black;

        private void Reset()
        {
            flammable = GetComponentInParent<Flammable>();
            target = GetComponentInChildren<Renderer>();
        }

        #endregion

        private Color initialColor;

        //private float highestIntensity = 0;

        private void Start()
        {
            flammable.onStartBurning.AddListener(OnStartBurning);
            flammable.onBurnDown.AddListener(OnBurnDown);
        }

        private void OnStartBurning()
        {
            initialColor = target.material.color;
        }

        private void OnBurnDown()
        {
            enabled = false;
        }

        private void LateUpdate()
        {
            if (!flammable.IsBurning && !flammable.IsBurntDown)
                return;

            //if (flammable.normalizedBurningIntensity > highestIntensity)
            //    highestIntensity = flammable.normalizedBurningIntensity;

            //target.material.color = Color.Lerp(initialColor, charredColor, highestIntensity);

            target.material.color = Color.Lerp(initialColor, charredColor, flammable.normalizedBurningIntensity);
        }
    }
}