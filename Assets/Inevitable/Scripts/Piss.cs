using DG.Tweening;
using JK.World;
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
    public class Piss : MonoBehaviour
    {
        #region Inspector

        public new Renderer renderer;

        public float secondsToBottom = 1;

        public AnimationCurve curveToBottom;

        public Transform body;

        public Transform follow;

        public Vector3 followOffset = Vector3.zero;

        public float lerp = 0.1f;

        public float thirstMultiplier = 1;

        public float thirstThreshold = 0.3f;

        public float thirstSeconds = 10;

        public float inputThirstSecondsBump = 0.2f;

        public float flameIntensityBumpPerSecond = 0.3f;

        public AudioClip startClip;

        public AudioSource loopSource;

        [SerializeField]
        private List<LerpToTarget> particles = null;

        public UnityEvent onDrink = new UnityEvent();

        private void Reset()
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        #endregion

        private List<ContactPoint> contactsList = new List<ContactPoint>(32);

        //private bool finishedToBottom = false;

        //[Inject]
        //private RotationActuatorInputBehaviour rotationInput = null;

        [Inject(Id = "sounds")]
        private AudioSource sounds = null;

        [InjectOptional(Id = "piss")]
        private Slider pissSlider = null;

        private void Start()
        {
            sounds.PlayOneShot(startClip);
            float targetVolume = loopSource.volume;
            loopSource.volume = 0;
            loopSource.Play();
            loopSource.DOFade(targetVolume, 1.75f).SetDelay(2);

            if (body.TryGetComponent(out ColliderStayEvent stay))
                stay.onCollisionStay.AddListener(OnCollision);
        }

        private void OnEnable()
        {
            StartCoroutine(PeeToBottomCoroutine());
        }

        private IEnumerator PeeToBottomCoroutine()
        {
            //finishedToBottom = false;
            float elapsed = 0;

            while (elapsed < secondsToBottom)
            {
                renderer.material.SetFloat("Cutoff", curveToBottom.Evaluate(elapsed /secondsToBottom));
                elapsed += Time.deltaTime;
                yield return null;
            }

            //finishedToBottom = true;
        }

        private void OnCollision(Collider collider, Collision collision)
        {
            contactsList.Clear();

            for (int i = 0; i < collision.contactCount; i++)
                contactsList.Add(collision.GetContact(i));

            if (collision.gameObject.TryGetComponent(out FlammableCollider flammable))
                flammable.target.normalizedBurningIntensity -= flameIntensityBumpPerSecond * Time.deltaTime; 
        }

        private void Update()
        {
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
                LerpToTarget particle = particles[i];

                if (i >= contactsList.Count)
                {
                    particle.gameObject.SetActive(false);
                }
                else
                {
                    particle.gameObject.SetActive(true);
                    particle.target = contactsList[i].point;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                thirstMultiplier = Mathf.Min(1, thirstMultiplier + inputThirstSecondsBump);
                onDrink.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (follow == null)
                return;

            body.position = Vector3.Lerp(body.position, follow.position + body.TransformDirection(followOffset), lerp);

            var followEuler = follow.eulerAngles;

            var euler = body.eulerAngles;
            euler.y = Mathf.LerpAngle(euler.y, followEuler.y, lerp);
            body.eulerAngles = euler;

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

            float actualThirstMultiplier = 1;
            thirstMultiplier = Mathf.Max(0, thirstMultiplier - Time.deltaTime / thirstSeconds);

            if (pissSlider != null)
                pissSlider.value = thirstMultiplier;

            if (thirstMultiplier < thirstThreshold)
                actualThirstMultiplier = thirstMultiplier / thirstThreshold;

            var scale = body.localScale;
            scale.z = Mathf.Lerp(0.5f, 4.7f, actualThirstMultiplier * (45 - rotX) / 45);
            body.localScale = Vector3.Lerp(body.localScale, scale, lerp);
        }

        public void EndFlow()
        {
            enabled = false;
            body.DOScaleZ(0, 1);
            loopSource.DOFade(0, 1);

            foreach (var particle in particles)
                particle.GetComponent<ParticleSystem>().Stop();
        }
    }
}