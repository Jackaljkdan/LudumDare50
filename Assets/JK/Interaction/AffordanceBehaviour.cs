using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public abstract class AffordanceBehaviour : MonoBehaviour, IAffordance
    {
        #region Inspector



        #endregion

        public bool IsHighlighting { get; private set; }

        private Coroutine coroutine;

        // declare a start method so that the monobehaviour can be disabled in in the inspector if needed
        protected virtual void Start() { }

        public void Highlight(RaycastHit hit)
        {
            if (!IsHighlighting)
            {
                //Debug.Log($"starting {name} highlight");
                StartHighlight(hit);
                IsHighlighting = true;
            }
            else
            {
                StayHighlight(hit);
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(StopHightlightingCoroutine());
        }

        private IEnumerator StopHightlightingCoroutine()
        {
            yield return null;
            yield return null;

            //Debug.Log($"stopping {name} highlight");
            StopHighlight();
            IsHighlighting = false;
            coroutine = null;
        }

        protected abstract void StartHighlight(RaycastHit hit);
        protected abstract void StayHighlight(RaycastHit hit);
        protected abstract void StopHighlight();
    }
    
    [Serializable]
    public class UnityEventAffordanceBehaviour : UnityEvent<AffordanceBehaviour> { }
}