using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public interface IAffordance
    {
        void Highlight(RaycastHit hit);
    }
    
    [Serializable]
    public class UnityEventIAffordance : UnityEvent<IAffordance> { }
}