using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class MathUtils
    {
        public static float RandomExponential(float k)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * 1 / k;
        }

        /// <summary>
        /// Interpret the parameter as the average time between two events
        /// </summary>
        public static float RandomExponentialInversed(float k)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * k;
        }
    }
}