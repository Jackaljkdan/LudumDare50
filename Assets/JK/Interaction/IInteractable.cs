using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public interface IInteractable
    {
        void Interact(RaycastHit hit);
    }
    
    [Serializable]
    public class UnityEventIInteractable : UnityEvent<IInteractable> { }
}