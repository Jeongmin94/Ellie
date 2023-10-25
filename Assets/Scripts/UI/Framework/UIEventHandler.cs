using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Framework
{
    // Unity Event System Interfaces
    // https://docs.unity.cn/Packages/com.unity.ugui@1.0/api/UnityEngine.EventSystems.html
    public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        // UIManager의 UIEvent 종류에 맞게 생성
        public Action<PointerEventData> OnClickHandler;
        public Action<PointerEventData> OnDragHandler;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragHandler?.Invoke(eventData);
        }
    }
}