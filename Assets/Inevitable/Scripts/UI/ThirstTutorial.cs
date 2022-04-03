using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Inevitable.UI
{
    [DisallowMultipleComponent]
    public class ThirstTutorial : MonoBehaviour
    {
        #region Inspector

        public AudioClip clip;

        #endregion

        [Inject]
        private Piss piss = null;

        [Inject(Id = "subtitles")]
        private CanvasGroup subtitlesGroup = null;

        [Inject(Id = "subtitles")]
        private Text subtitles = null;

        [Inject(Id = "sounds")]
        private AudioSource sounds = null;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (piss.thirstMultiplier >= piss.thirstThreshold)
                return;

            enabled = false;

            StartCoroutine(ShowTutorialCoroutine());
        }

        private IEnumerator ShowTutorialCoroutine()
        {
            sounds.PlayOneShot(clip);

            subtitles.text = "Mash the left mouse button to fill your bladder!";
            subtitlesGroup.DOFade(1, 0.5f);

            yield return new WaitForSeconds(3);

            subtitlesGroup.DOFade(0, 0.5f);

            Destroy(gameObject);
        }
    }
}