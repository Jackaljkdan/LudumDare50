using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class HandlesUtils
    {
        public static void Label(Vector3 position, string text, TextAnchor alignment = TextAnchor.MiddleCenter, Color? color = null)
        {
#if UNITY_EDITOR
            var style = new GUIStyle();
            style.alignment = alignment;
            style.normal.textColor = color ?? Color.white;
            UnityEditor.Handles.Label(position, text, style);
#endif
        }
    }
}