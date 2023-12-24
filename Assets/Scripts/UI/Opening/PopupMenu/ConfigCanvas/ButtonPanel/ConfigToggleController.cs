using System;
using UI.Inventory;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine.UI;

namespace UI.Opening.PopupMenu.ConfigCanvas.ButtonPanel
{
    public class ConfigToggleController : ToggleController
    {
        public static readonly string Path = "Opening/ConfigToggleText";
        private Action<PopupPayload> toggleAction;

        public ConfigType ToggleConfigType { get; set; }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            toggleAction = null;
        }

        public void Subscribe(Action<PopupPayload> listener)
        {
            toggleAction -= listener;
            toggleAction += listener;
        }

        public void Init()
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            var payload = new PopupPayload();
            payload.configType = ToggleConfigType;
            payload.isOn = isOn;
            toggleAction?.Invoke(payload);
        }

        public void SetToggleGroup(ToggleGroup toggleGroup)
        {
            toggle.group = toggleGroup;
        }
    }
}