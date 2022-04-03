using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class Building : MonoBehaviour
    {
        #region Inspector



        #endregion

        public bool IsBurntDown => flammables != null ? burntCount >= flammables.Count : false;

        private List<Flammable> flammables;

        private int burntCount = 0;

        private void Start()
        {
            foreach (Flammable flammable in EnumerateFlammables())
                flammable.onBurnDown.AddListener(OnFlammableBurntDown);
        }

        private void OnFlammableBurntDown()
        {
            burntCount++;

            if (burntCount == flammables.Count)
            {
                Debug.Log(name + " burnt down");
                gameObject.SetActive(false);
            }
        }

        public IEnumerable<Flammable> EnumerateFlammables()
        {
            if (flammables == null)
            {
                flammables = new List<Flammable>(transform.childCount);

                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent(out Flammable flammable))
                        flammables.Add(flammable);
                }
            }

            foreach (Flammable flam in flammables)
                if (flam != null)
                    yield return flam;
        }
    }
}