using System;
using Assets.Scripts.UI.Inventory;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigToggleController : ToggleController
    {
        public static readonly string Path = "Opening/ConfigToggleText";

        public ConfigType ToggleConfigType { get; set; }
        private Action<PopupPayload> toggleAction;

        public void Subscribe(Action<PopupPayload> listener)
        {
            toggleAction -= listener;
            toggleAction += listener;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            toggleAction = null;
        }

        public void Init()
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            PopupPayload payload = new PopupPayload();
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