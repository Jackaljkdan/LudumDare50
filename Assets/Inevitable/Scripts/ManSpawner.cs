using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class ManSpawner : MonoBehaviour
    {
        #region Inspector

        public Transform spawnAnchor;

        public Man manPrefab;

        #endregion

        [Inject]
        private DiContainer diContainer = null;

        private void Start()
        {
            foreach (var fc in GetComponentsInChildren<FlammableCollider>())
                fc.onHit.AddListener(OnHit);
        }

        private void OnHit()
        {
            Man man = Instantiate(manPrefab);
            diContainer.InjectGameObject(man.gameObject);

            man.transform.position = spawnAnchor.position;
            man.transform.forward = spawnAnchor.forward;
        }
    }
}