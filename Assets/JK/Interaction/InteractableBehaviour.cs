using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public abstract class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        #region Inspector

        

        #endregion

        public void Interact(RaycastHit hit)
        {
            PerformInteraction(hit);
        }

        protected abstract void PerformInteraction(RaycastHit hit);
    }
    
    [Serializable]
    public class UnityEventInteractableBehaviour : UnityEvent<InteractableBehaviour> { }
}