using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public class InteractionForwarder : MonoBehaviour, IInteractable
    {
        #region Inspector

        public InteractableBehaviour target;

        #endregion

        public void Interact(RaycastHit hit)
        {
            if (target != null)
                target.Interact(hit);
        }
    }
    
    [Serializable]
    public class UnityEventInteractionForwarder : UnityEvent<InteractionForwarder> { }
}