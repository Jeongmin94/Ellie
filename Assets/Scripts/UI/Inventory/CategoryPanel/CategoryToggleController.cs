using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class CategoryToggleController : ToggleController
    {
        public string toggleTitle;

        private TextMeshProUGUI text;

        private void Awake()
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);

            text = gameObject.FindChild<TextMeshProUGUI>(null, true);
            text.alignment = TextAlignmentOptions.Midline;
            text.text = toggleTitle;
            text.lineSpacing = 25.0f;

            text.fontSize = GetToggledSize(toggle.isOn);
            text.color = GetToggledColor(toggle.isOn);
        }

        private void OnValueChanged(bool isOn)
        {
            text.fontSize = GetToggledSize(isOn);
            text.color = GetToggledColor(isOn);
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