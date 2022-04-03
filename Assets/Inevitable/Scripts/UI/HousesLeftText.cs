using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Inevitable.UI
{
    [DisallowMultipleComponent]
    public class HousesLeftText : MonoBehaviour
    {
        #region Inspector

        public Text target;

        private void Reset()
        {
            target = GetComponent<Text>();
        }

        #endregion

        [Inject(Id = "targets")]
        private List<Building> buildings = null;

        private int left;
        private int total;

        private void Start()
        {
            foreach (var building in buildings)
                building.onBurntDown.AddListener(OnBurnt);

            total = left = buildings.Count;
            Refresh();
        }

        private void OnBurnt()
        {
            left--;
            Refresh();
        }

        private void Refresh()
        {
            target.text = $"Houses left: {left}/{total}";
        }
    }
}