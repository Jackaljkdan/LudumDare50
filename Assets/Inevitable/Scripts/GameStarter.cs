using DG.Tweening;
using Inevitable.UI;
using JK.Actuators.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class GameStarter : MonoBehaviour
    {
        #region Inspector

        public GameObject sky;

        public FireballSpawner introSpawner;
        public FireballSpawner scenicSpawner;

        public AudioClip introClip;

        public bool skipInEditor = true;

        #endregion

        [Inject]
        private Piss piss = null;

        [Inject]
        private Timer timer = null;

        [Inject(Id = "music")]
        private AudioSource music = null;

        [Inject(Id = "sounds")]
        private AudioSource sounds = null;

        [Inject(Id = "subtitles")]
        private Text subtitles = null;

        [Inject(Id = "subtitles")]
        private CanvasGroup subtitlesGroup = null;

        [Inject]
        private RotationActuatorInputBehaviour rotationInput = null;

        private void Start()
        {
            rotationInput.enabled = false;
            subtitlesGroup.alpha = 0;
            if (timer.TryGetComponent(out CanvasGroup group))
                group.alpha = 0;
        }

        private void Update()
        {
            if (!UnityEngine.Input.anyKeyDown)
                return;

            enabled = false;

            StartCoroutine(GameStartCoroutine());
        }

        private IEnumerator GameStartCoroutine()
        {
            if (!skipInEditor)
            {
                scenicSpawner.gameObject.SetActive(true);

                introSpawner.targetNoise = 0;
                introSpawner.Spawn();

                yield return new WaitForSeconds(2);

                sounds.PlayOneShot(introClip);

                yield return new WaitForSeconds(0.5f);

                subtitles.text = string.Empty;
                yield return subtitlesGroup.DOFade(1, 0.5f).WaitForCompletion();

                yield return subtitles.DOText("Oh no! Your Evil rival God is attacking our village! He's casting a Firestorm on us!", duration: 7).SetEase(Ease.Linear).WaitForCompletion();
                yield return new WaitForSeconds(2);

                subtitles.text = string.Empty;
                yield return subtitles.DOText("Please, we pray you All Mighty, use your Divine Extinguesher and save us!", duration: 7).SetEase(Ease.Linear).WaitForCompletion();
                yield return new WaitForSeconds(2);

                subtitles.text = string.Empty;
                yield return subtitles.DOText("*finishes smoking*", duration: 1).SetEase(Ease.Linear).WaitForCompletion();
                yield return new WaitForSeconds(2);

                subtitles.text = string.Empty;
                yield return subtitles.DOText("Well, I don't seem to have my Divine Extinguisher at hand...", duration: 4).SetEase(Ease.Linear).WaitForCompletion();
                yield return new WaitForSeconds(2);

                subtitles.text = string.Empty;
                subtitles.DOText("But yes, in the end I can feel some Divine Rain that needs to be let out, if you know what i mean...", duration: 9).SetEase(Ease.Linear).WaitForCompletion();
                yield return new WaitForSeconds(6);

                introSpawner.Spawn();
                Destroy(introSpawner.gameObject);

                yield return new WaitForSeconds(3);
            }

            rotationInput.enabled = true;
            sky.SetActive(true);
            timer.StartTimer();
            if (timer.TryGetComponent(out CanvasGroup group))
                group.DOFade(1, 0.5f);

            if (!skipInEditor)
                yield return new WaitForSeconds(1);

            piss.gameObject.SetActive(true);
            music.Play();

            if (!skipInEditor)
            {
                subtitlesGroup.DOFade(0, 0.5f);
                yield return new WaitForSeconds(2);
            }

            Destroy(gameObject);
        }
    }
}