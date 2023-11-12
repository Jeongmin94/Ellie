using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Equipment
{
    public class EquipmentFrame : UIBase
    {
        public static readonly string DefaultPath = "Slot/Equipment/EquipmentFrame";
        public static readonly string StonePath = "Slot/Equipment/StoneEquipmentFrame";

        private enum Images
        {
            ItemImage
        }

        private enum Texts
        {
            ItemText
        }

        public Transform ImageRect { get; set; }

        private Image frameImage;
        private RectTransform frameRect;

        private Image itemImage;
        private TextMeshProUGUI itemText;

        private Color offColor = Color.white;
        private Color onColor = Color.white;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            frameImage = GetComponent<Image>();
            frameRect = GetComponent<RectTransform>();

            itemImage = GetImage((int)Images.ItemImage);
            itemText = GetText((int)Texts.ItemText);
        }

        private void InitObjects()
        {
            onColor.a = 1.0f;
            offColor.a = 0.0f;

            ImageRect = itemImage.GetComponent<RectTransform>();

            TurnOffFrameItem();
        }

        public void TurnOffFrameItem()
        {
            itemImage.color = offColor;
            itemText.color = offColor;
        }

        public void TurnOnFrameItem()
        {
            itemImage.color = onColor;
            itemText.color = onColor;
        }

        public void SetFrame(float width, float height)
        {
            AnchorPresets.SetAnchorPreset(frameRect, AnchorPresets.MiddleCenter);
            frameRect.sizeDelta = new Vector2(width, height);
            frameRect.localPosition = Vector2.zero;
        }

        public void SetFrameImage(Sprite sprite)
        {
            frameImage.sprite = sprite;
        }

        public void SetItemImage(Sprite sprite)
        {
            itemImage.sprite = sprite;
        }

        public void SetItemText(string text)
        {
            itemText.text = text;
        }
    }
}