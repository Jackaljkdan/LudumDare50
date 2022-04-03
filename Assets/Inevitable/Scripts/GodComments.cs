using DG.Tweening;
using Inevitable.UI;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class GodComments : MonoBehaviour
    {
        #region Inspector

        public ThirstTutorial tutorial;
        public DrinkFeedback drinkFeedback;

        public List<AudioClip> burnClips;
        public List<AudioClip> savedClips;

        public float secondsBetweenComments = 10;

        #endregion

        public bool IsCommenting { get; private set; }

        [Inject(Id = "sounds")]
        private AudioSource sounds = null;

        [Inject(Id = "music")]
        private AudioSource music = null;

        [Inject(Id = "targets")]
        private List<Building> buildings = null;

        [Inject]
        private SignalBus signalBus = null;

        private float lastPositiveCommentTime;
        private float lastNegativeCommentTime;
        private int burnIndex = 0;
        private int savedIndex = 0;

        private void Start()
        {
            foreach (var building in buildings)
                building.onBurntDown.AddListener(OnBurnt);

            signalBus.Subscribe<FlammableStoppedBurningSignal>(OnStopBurning);
            signalBus.Subscribe<EndgameSignal>(OnEndGame);

            ListUtils.ShuffleInPlace(burnClips);
            ListUtils.ShuffleInPlace(savedClips);
        }

        private void OnStopBurning()
        {
            if (CanComment(positive: true))
                Comment(positive: true);
        }

        private void OnBurnt()
        {
            if (CanComment(positive: false))
                Comment(positive: false);
        }

        private void OnEndGame()
        {
            enabled = false;
        }

        public bool CanComment(bool positive)
        {
            if (!enabled)
                return false;

            if (IsCommenting)
                return false;

            if (tutorial != null && tutorial.IsPlayingTutorial)
                return false;

            if (drinkFeedback.IsPlaying)
                return false;

            float lastCommentTime = positive ? lastPositiveCommentTime : lastNegativeCommentTime;
            return Time.time - lastCommentTime > secondsBetweenComments;
        }

        public void Comment(bool positive)
        {
            ref float lastCommentTime = ref lastPositiveCommentTime;
            ref int index = ref savedIndex;
            List<AudioClip> list = savedClips;

            if (!positive)
            {
                lastCommentTime = ref lastNegativeCommentTime;
                index = ref burnIndex;
                list = burnClips;
            }

            AudioClip clip = list[index];
            index = (index + 1) % list.Count;

            if (index == 0)
                ListUtils.ShuffleInPlace(list);

            lastCommentTime = Time.time + clip.length;

            music.DOFade(0.2f, 0.5f);

            sounds.PlayOneShot(clip);

            IsCommenting = true;
            Invoke(nameof(StopCommenting), clip.length);
        }

        private void StopCommenting()
        {
            if (enabled)
                music.DOFade(1, 0.5f);

            IsCommenting = false;
        }
    }
}