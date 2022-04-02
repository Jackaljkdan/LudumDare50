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

        [Inject]
        private RotationActuatorInputBehaviour rotationInput = null;

        private void Start()
        {
            rotationInput.enabled = false;
            subtitles.text = string.Empty;
        }

        private void Update()
        {
            if (!UnityEngine.Input.anyKeyDown)
                return;

            StartCoroutine(GameStartCoroutine());
        }

        private IEnumerator GameStartCoroutine()
        {
            introSky.SetActive(true);

            yield return new WaitForSeconds(1);

            sounds.PlayOneShot(introClip);

            yield return new WaitForSeconds(1);

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

            yield return new WaitForSeconds(2);

            Destroy(gameObject);
        }
    }
}