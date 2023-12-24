using System;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine;

namespace UI.Opening.PopupMenu.MenuButton
{
    public abstract class MenuButton : MonoBehaviour
    {
        private Action<PopupPayload> menuButtonAction;

        protected virtual void OnDestroy()
        {
            menuButtonAction = null;
        }

        public void Subscribe(Action<PopupPayload> listener)
        {
            menuButtonAction -= listener;
            menuButtonAction += listener;
        }

        protected void Invoke(PopupPayload payload)
        {
            menuButtonAction?.Invoke(payload);
        }

        public abstract void Click();
    }
}