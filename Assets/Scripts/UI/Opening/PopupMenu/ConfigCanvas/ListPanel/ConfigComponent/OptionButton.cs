using System;
using UI.Framework;
using UnityEngine.EventSystems;
using Utils;

namespace UI.Opening.PopupMenu.ConfigCanvas.ListPanel.ConfigComponent
{
    public class OptionButton : UIBase
    {
        private Action<int> buttonAction;
        private int buttonValue;

        private void OnDestroy()
        {
            buttonAction = null;
        }

        public void InitOptionButton(int direction)
        {
            buttonValue = direction;
            Init();
        }

        public void Subscribe(Action<int> listener)
        {
            buttonAction -= listener;
            buttonAction += listener;
        }

        protected override void Init()
        {
            BindEvent();
        }

        private void BindEvent()
        {
            gameObject.BindEvent(OnClickHandler);
        }

        private void OnClickHandler(PointerEventData data)
        {
            buttonAction?.Invoke(buttonValue);
        }
    }
}