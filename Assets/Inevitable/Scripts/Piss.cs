using JK.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class Piss : MonoBehaviour
    {
        #region Inspector

        public new Renderer renderer;

        public float secondsToBottom = 1;

        [SerializeField]
        private List<GameObject> particles = null;

        private void Reset()
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        #endregion

        private List<ContactPoint> contactsList = new List<ContactPoint>(10);

        private void Start()
        {
            renderer.material.SetFloat("Cutoff", 1);
        }

        private void OnCollisionStay(Collision collision)
        {
            contactsList.Clear();

            for (int i = 0; i < collision.contactCount; i++)
                contactsList.Add(collision.GetContact(i));

            contactsList.Sort((cp1, cp2) => 
                cp1.point.y < cp2.point.y
                    ? 1
                    : cp1.point.y == cp2.point.y
                        ? 0
                        : -1
            );

            Debug.Log($"contacts {string.Join(" ", contactsList.Select(cp => cp.point.y.ToString("0.0")))}");

            for (int i = 0; i < particles.Count; i++)
            {
                if (i >= contactsList.Count)
                {
                    particles[i].gameObject.SetActive(false);
                }
                else
                {
                    particles[i].gameObject.SetActive(true);
                    particles[i].transform.position = contactsList[i].point;
                }
            }
        }
    }
}