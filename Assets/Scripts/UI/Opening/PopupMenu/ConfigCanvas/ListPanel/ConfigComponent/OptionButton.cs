using System;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using UnityEngine.EventSystems;

namespace Data.UI.Config
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