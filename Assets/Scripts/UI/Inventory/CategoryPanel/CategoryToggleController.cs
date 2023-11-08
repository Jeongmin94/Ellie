using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class CategoryToggleController : ToggleController
    {
        private ToggleChangeHandler toggleChangeCallback;

        public string toggleTitle;
        public GroupType type;

        private Graphic checkMark;
        private TextMeshProUGUI text;
        private Color pressedColor;
        private Color normalColor;

        private void OnDestroy()
        {
            toggleChangeCallback = null;
        }

        public void Init(GroupType groupType)
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);

            checkMark = toggle.graphic;

            text = gameObject.FindChild<TextMeshProUGUI>(null, true);
            text.alignment = TextAlignmentOptions.Midline;
            text.text = toggleTitle;
            text.lineSpacing = 25.0f;

            text.fontSize = GetToggledSize(IsOn);
            text.color = GetToggledColor(IsOn);

            type = groupType;
            pressedColor = toggle.colors.pressedColor;
            normalColor = toggle.colors.normalColor;

            gameObject.BindEvent(OnDownHandler, UIEvent.Down);
            gameObject.BindEvent(OnUpHandler, UIEvent.Up);
        }

        private void OnDownHandler(PointerEventData data)
        {
            checkMark.color = pressedColor;
            text.color = InventoryConst.ToggleOffFontColor;
        }

        private void OnUpHandler(PointerEventData data)
        {
            checkMark.color = normalColor;
            text.color = InventoryConst.ToggleOnFontColor;
        }

        public void Subscribe(ToggleChangeHandler listener)
        {
            toggleChangeCallback -= listener;
            toggleChangeCallback += listener;
        }

        private void OnValueChanged(bool isOn)
        {
            text.fontSize = GetToggledSize(isOn);
            text.color = GetToggledColor(isOn);
            Interactable = !isOn;
            toggleChangeCallback?.Invoke(ToggleChangeInfo.Of(type, isOn));
        }

        private Color GetToggledColor(bool isOn)
        {
            return isOn ? InventoryConst.ToggleOnFontColor : InventoryConst.ToggleOffFontColor;
        }

        private int GetToggledSize(bool isOn)
        {
            return isOn ? InventoryConst.ToggleOnFontSize : InventoryConst.ToggleOffFontSize;
        }

        public void ActivateToggle(bool isOn)
        {
            toggle.isOn = isOn;
            toggle.onValueChanged?.Invoke(isOn);
        }
    }
}