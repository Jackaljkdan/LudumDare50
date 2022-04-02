using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Particles
{
    [DisallowMultipleComponent]
    public class MatchParticlesScale : MonoBehaviour
    {
        #region Inspector

        public ParticleSystem target;

        public bool matchOnStart = false;

        #endregion

        private void Start()
        {
            if (matchOnStart)
                MatchScale();
        }

        [ContextMenu("Match Scale")]
        public void MatchScale()
        {
            if (target == null)
                return;

            Undo.RecordObjectForUndo(target.gameObject, "Match Particles Scale");
            var shape = target.shape;
            shape.scale = transform.localScale;
        }
    }
}