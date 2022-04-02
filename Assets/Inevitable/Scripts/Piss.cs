using JK.Actuators.Input;
using JK.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Inevitable
{
    [DisallowMultipleComponent]
    public class Piss : MonoBehaviour
    {
        #region Inspector

        public new Renderer renderer;

        public float secondsToBottom = 1;

        public AnimationCurve curveToBottom;

        public Transform follow;

        public Vector3 followOffset = Vector3.zero;

        public float lerp = 0.1f;

        [SerializeField]
        private List<GameObject> particles = null;

        private void Reset()
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        #endregion

        private List<ContactPoint> contactsList = new List<ContactPoint>(10);

        private bool finishedToBottom = false;

        //[Inject]
        //private RotationActuatorInputBehaviour rotationInput = null;

        private void OnEnable()
        {
            StartCoroutine(PeeToBottomCoroutine());
        }

        private IEnumerator PeeToBottomCoroutine()
        {
            finishedToBottom = false;
            float elapsed = 0;

            while (elapsed < secondsToBottom)
            {
                renderer.material.SetFloat("Cutoff", curveToBottom.Evaluate(elapsed /secondsToBottom));
                elapsed += Time.deltaTime;
                yield return null;
            }

            finishedToBottom = true;
        }

        private void LateUpdate()
        {
            if (follow == null)
                return;

            Transform tr = transform;

            tr.position = Vector3.Lerp(tr.position, follow.position + tr.TransformDirection(followOffset), lerp);

            var followEuler = follow.eulerAngles;

            var euler = tr.eulerAngles;
            euler.y = Mathf.LerpAngle(euler.y, followEuler.y, lerp);
            tr.eulerAngles = euler;

            // x rot 0 => scale 4.7
            // x rot 45 => scale 1

            float rotX = followEuler.x;
            if (rotX > 45)
            {
                if (rotX < 90)
                    rotX = 45;
                else if (rotX >= 90)
                    rotX = 0;
            }

            tr.localScale = Vector3.Lerp(tr.localScale, new Vector3(1, 1, Mathf.Lerp(4.7f, 0.5f, rotX/45)), lerp);
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

            //Debug.Log($"contacts {string.Join(" ", contactsList.Select(cp => cp.point.y.ToString("0.0")))}");

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