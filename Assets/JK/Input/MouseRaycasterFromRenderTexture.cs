using JK.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JK.Input
{
    public class MouseRaycasterFromRenderTexture : MonoBehaviour
    {
        #region Inspector fields

        public PointerEventData.InputButton mouseButton = PointerEventData.InputButton.Left;

        public LayerMask layerMask = 0x7FFFFFFF;

        public float maxDistance = float.PositiveInfinity;

        public new Camera camera = null;

        public Canvas canvas = null;

        public RawImage renderImage = null;

        public UnityEventMouseRaycasterFromRenderTextureHit onRaycastHit = new UnityEventMouseRaycasterFromRenderTextureHit();

        #endregion

        private void Start()
        {
            var events = renderImage.GetComponent<PointerEvents>();

            Vector2 imgPivot = renderImage.rectTransform.pivot;
            if (imgPivot.x != 0.5f || imgPivot.y != 0.5f)
                Debug.LogError($"{nameof(renderImage)} has pivot {imgPivot} that will not work properly with {GetType().Name}, set the pivot to (0.5, 0.5)");

            events.onPointerClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            if (renderImage != null)
            {
                var events = renderImage.GetComponent<PointerEvents>();
                events.onPointerClick.AddListener(OnClicked);
            }
        }

        private Ray ray;

        private void OnClicked(PointerEvents _, PointerEventData eventData)
        {
            if (!enabled)
                return;

            if (eventData.button != mouseButton)
                return;

            RectTransform imageRt = renderImage.rectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRt, eventData.position, null, out Vector2 point);

            Vector2 viewport = new Vector2(
                0.5f + point.x / imageRt.rect.width,
                0.5f + point.y / imageRt.rect.height
            );

            //Debug.Log($"pos {eventData.position} point {point} vw {viewport}");

            ray = camera.ViewportPointToRay(viewport);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
            {
                //Debug.Log($"hit {hit.collider.name}");
                onRaycastHit?.Invoke(this, hit);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 1000);
        }
    }

    [Serializable]
    public class UnityEventMouseRaycasterFromRenderTexture : UnityEvent<MouseRaycasterFromRenderTexture> { }

    [Serializable]
    public class UnityEventMouseRaycasterFromRenderTextureHit : UnityEvent<MouseRaycasterFromRenderTexture, RaycastHit> { }
}