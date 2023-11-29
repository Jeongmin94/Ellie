using System;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class PopupCanvas : MonoBehaviour
    {
        private Action<PopupPayload> popupCanvasAction;

        public void Subscribe(Action<PopupPayload> listener)
        {
            popupCanvasAction -= listener;
            popupCanvasAction += listener;
        }

        public virtual void Invoke(PopupPayload payload)
        {
            popupCanvasAction?.Invoke(payload);
        }
        
        protected virtual void OnDestroy()
        {
            popupCanvasAction = null;
        }
    }
}