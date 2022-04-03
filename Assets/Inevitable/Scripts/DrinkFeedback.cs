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

        public GodComments godComments;

        public AudioClip clip;

        public float secondsBetweenSounds = 10f;

        #endregion

        public bool IsPlaying { get; private set; }

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

            if (godComments.IsCommenting)
                return;

            if (Time.time - lastSoundTime > secondsBetweenSounds)
                PlaySound();
        }

        public void PlaySound()
        {
            sounds.PlayOneShot(clip);
            lastSoundTime = Time.time + clip.length;

            IsPlaying = true;
            Invoke(nameof(StopPlaying), clip.length);
        }

        private void StopPlaying()
        {
            IsPlaying = false;
        }
    }
}