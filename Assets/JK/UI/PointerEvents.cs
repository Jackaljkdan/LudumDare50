using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JK.UI
{
    public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Inspector fields

        public UnityEventPointerEvents onPointerEnter;
        public UnityEventPointerEvents onPointerExit;
        public UnityEventPointerEvents onPointerClick;

        #endregion

        public RectTransform GetRectTransform()
        {
            return GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke(this, eventData);
        }
    }

    [Serializable]
    public class UnityEventPointerEvents : UnityEvent<PointerEvents, PointerEventData> { }
}