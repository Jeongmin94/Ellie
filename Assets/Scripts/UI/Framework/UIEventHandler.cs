using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI.Framework
{
    // Unity Event System Interfaces
    // https://docs.unity.cn/Packages/com.unity.ugui@1.0/api/UnityEngine.EventSystems.html
    public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler
    {
        // UIManager의 UIEvent 종류에 맞게 생성
        public Action<PointerEventData> clickHandlerAction;
        public Action<PointerEventData> downHandlerAction;
        public Action<PointerEventData> upHandlerAction;
        public Action<PointerEventData> dragHandlerAction;
        public Action<PointerEventData> beginDragHandlerAction;
        public Action<PointerEventData> endDragHandlerAction;
        public Action<PointerEventData> dropHandlerAction;

        public void OnPointerClick(PointerEventData eventData)
        {
            clickHandlerAction?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragHandlerAction?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            beginDragHandlerAction?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            endDragHandlerAction?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            dropHandlerAction?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            downHandlerAction?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            upHandlerAction?.Invoke(eventData);
        }
    }
}