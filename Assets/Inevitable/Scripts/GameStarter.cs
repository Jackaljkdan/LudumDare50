using DG.Tweening;
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

        public Piss piss;

        public GameObject sky;

        public GameObject introSky;

        public AudioClip introClip;

        #endregion

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
            introSky.SetActive(true);

            yield return new WaitForSeconds(1);

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
            yield return subtitles.DOText("But yes, in the end I can feel some Divine Rain that needs to be let out, if you know what i mean...", duration: 9).SetEase(Ease.Linear).WaitForCompletion();

            rotationInput.enabled = true;
            sky.SetActive(true);

            yield return new WaitForSeconds(2);
            subtitles.text = string.Empty;

            piss.gameObject.SetActive(true);
            music.Play();

            subtitlesGroup.DOFade(0, 0.5f);
            yield return new WaitForSeconds(2);

            Destroy(gameObject);
        }
    }
}