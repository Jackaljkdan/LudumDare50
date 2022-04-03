using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inevitable.UI
{
    [DisallowMultipleComponent]
    public class Timer : MonoBehaviour
    {
        #region Inspector

        public Text target;

        private void Reset()
        {
            target = GetComponentInChildren<Text>();
        }

        #endregion

        private DateTime startTime;
        private DateTime endTime;

        private void Start()
        {
            enabled = false;
        }

        public void StartTimer()
        {
            startTime = DateTime.Now;
            enabled = true;
        }

        public void EndTimer()
        {
            enabled = false;
            endTime = DateTime.Now;
        }

        public TimeSpan GetElapsed()
        {
            return endTime - startTime;
        }

        private void Update()
        {
            TimeSpan span = DateTime.Now - startTime;
            double totalSeconds = span.TotalSeconds;
            long rounded = (long) Math.Round(totalSeconds);

            long seconds = rounded % 60;
            long minutes = rounded / 60;

            target.text = $"{minutes}:{seconds.ToString().PadLeft(2, '0')}";
        }
    }
}