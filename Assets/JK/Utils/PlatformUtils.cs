using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class PlatformUtils
    {
        public static bool IsMobile()
        {
            return Application.isMobilePlatform;
        }

        public static bool IsDesktop()
        {
            return !IsMobile();
        }
    }
}