using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigToggleController : ToggleController
    {
        public static readonly string Path = "Opening/ConfigToggleText";

        public void Init()
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);

            gameObject.BindEvent(OnDownHandler, UIEvent.Down);
            gameObject.BindEvent(OnUpHandler, UIEvent.Up);
        }

        private void OnValueChanged(bool isOn)
        {
            Interactable = !isOn;
        }

        private void OnDownHandler(PointerEventData data)
        {
        }

        private void OnUpHandler(PointerEventData data)
        {
        }

        public void SetToggleGroup(ToggleGroup toggleGroup)
        {
            toggle.group = toggleGroup;
        }
    }
}