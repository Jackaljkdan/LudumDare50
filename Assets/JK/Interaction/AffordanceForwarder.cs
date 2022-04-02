using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public class AffordanceForwarder : MonoBehaviour, IAffordance
    {
        #region Inspector

        public AffordanceBehaviour target;

        #endregion

        public void Highlight(RaycastHit hit)
        {
            if (target != null)
                target.Highlight(hit);
        }

    }
    
    [Serializable]
    public class UnityEventAffordanceForwarder : UnityEvent<AffordanceForwarder> { }
}