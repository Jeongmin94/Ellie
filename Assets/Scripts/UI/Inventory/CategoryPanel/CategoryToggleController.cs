using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class CategoryToggleController : ToggleController
    {
        private ToggleChangeHandler toggleChangeCallback;

        public string toggleTitle;
        public GroupType type;
        private TextMeshProUGUI text;

        public void Init(GroupType groupType)
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);

            text = gameObject.FindChild<TextMeshProUGUI>(null, true);
            text.alignment = TextAlignmentOptions.Midline;
            text.text = toggleTitle;
            text.lineSpacing = 25.0f;

            text.fontSize = GetToggledSize(IsOn);
            text.color = GetToggledColor(IsOn);

            type = groupType;
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
    }
}