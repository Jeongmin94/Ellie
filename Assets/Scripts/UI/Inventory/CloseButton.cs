using System;
using UI.Framework;
using UnityEngine.UI;
using Utils;

namespace UI.Inventory
{
    public class CloseButton : UIBase
    {
        public static readonly string Path = "Slot/CloseButton";

        private Button button;
        private Action clickAction;

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            clickAction = null;
        }

        protected override void Init()
        {
            button = gameObject.GetOrAddComponent<Button>();
            button.onClick.AddListener(OnClickButtonCallback);
        }

        public void Subscribe(Action listener)
        {
            clickAction -= listener;
            clickAction += listener;
        }

        private void OnClickButtonCallback()
        {
            clickAction?.Invoke();
        }
    }
}