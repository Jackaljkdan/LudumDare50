using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public interface IRef<T>
    {
        T Ref { get; }
    }
}