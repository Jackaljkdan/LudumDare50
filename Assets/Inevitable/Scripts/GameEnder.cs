using DG.Tweening;
using Inevitable.UI;
using JK.Actuators;
using JK.Actuators.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class GameEnder : MonoBehaviour
    {
        #region Inspector

        public AudioClip endgameClip;

        public bool skipInEditor = true;

        #endregion

        [Inject]
        private Piss piss = null;

        [Inject]
        private Timer timer = null;

        [Inject(Id = "targets")]
        private List<Building> buildings = null;

        private int burntCount = 0;

        [Inject(Id = "sounds")]
        private AudioSource sounds = null;

        [Inject(Id = "music")]
        private AudioSource music = null;

        [Inject(Id = "endgame")]
        private CanvasGroup endgame = null;

        [Inject]
        private InertialTransformRotationActuator rotation = null;

        private void Start()
        {
            endgame.gameObject.SetActive(false);

            foreach (var building in buildings)
                building.onBurntDown.AddListener(OnBuildingBurnt);
        }

        private void OnBuildingBurnt()
        {
            burntCount++;

            if (burntCount >= buildings.Count)
                EndGame();
        }

        [ContextMenu("End Game")]
        public void EndGame()
        {
            StartCoroutine(EndGameCoroutine());
        }

        private IEnumerator EndGameCoroutine()
        {
            piss.EndFlow();
            timer.EndTimer();

            music.DOFade(0, 1);
            sounds.PlayOneShot(endgameClip);

            if (!Application.isEditor || !skipInEditor)
                yield return new WaitForSeconds(5);

            endgame.alpha = 0;
            endgame.gameObject.SetActive(true);
            endgame.DOFade(1, 1);

            rotation.Speed = 5;

            Cursor.lockState = CursorLockMode.None;

            if (!Application.isEditor || !skipInEditor)
                yield return new WaitForSeconds(endgameClip.length - 5);

            if (Application.isEditor && skipInEditor)
                yield return new WaitForSeconds(endgameClip.length);

            music.DOFade(1, 1);

            Destroy(gameObject);
        }
    }
}