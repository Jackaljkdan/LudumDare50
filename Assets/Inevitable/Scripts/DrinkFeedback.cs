using Inevitable.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class DrinkFeedback : MonoBehaviour
    {
        #region Inspector

        public ThirstTutorial tutorial;

        public AudioClip clip;

        public float secondsBetweenSounds = 10f;

        #endregion

        [Inject]
        private Piss piss = null;

        [Inject(Id = "sounds")]
        private AudioSource sounds = null;

        private float lastSoundTime;

        private void Start()
        {
            piss.onDrink.AddListener(OnDrink);
        }

        private void OnDrink()
        {
            if (tutorial != null && tutorial.IsPlayingTutorial)
                return;

            if (Time.time > lastSoundTime + clip.length)
                PlaySound();
        }

        public void PlaySound()
        {
            sounds.PlayOneShot(clip);
            lastSoundTime = Time.time;
        }
    }
}