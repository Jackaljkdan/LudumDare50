using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public static class RandomUtils
    {
        /// <summary>
        /// The return value of the exponential distribution can be interpreted as the time to wait
        /// until the next event assuming that on average <paramref name="rate"/> events happen every
        /// time unit
        /// </summary>
        public static float Exponential(float rate)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * 1 / rate;
        }
    }
}