using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    public interface IRotationActuator
    {
        float Speed { get; set; }
        Vector2 Input { get; set; }
    }
    
    [Serializable]
    public class UnityEventIRotationActuator : UnityEvent<IRotationActuator> { }
}