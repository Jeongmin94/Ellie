using System;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public abstract class MenuButton : MonoBehaviour
    {
        private Action<PopupPayload> menuButtonAction;

        public void Subscribe(Action<PopupPayload> listener)
        {
            menuButtonAction -= listener;
            menuButtonAction += listener;
        }

        protected virtual void OnDestroy()
        {
            menuButtonAction = null;
        }

        protected void Invoke(PopupPayload payload)
        {
            menuButtonAction?.Invoke(payload);
        }

        public abstract void Click();
    }
}