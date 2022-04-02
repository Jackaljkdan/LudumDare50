using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Events
{
    [Serializable]
    public class UnityEventCollision : UnityEvent<Collider, Collision> { }
}