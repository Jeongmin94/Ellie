using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Equipment
{
    public class EquipmentFrame : UIBase
    {
        public static readonly string Path = "Slot/Equipment/EquipmentFrame";

        private Image frameImage;
        private RectTransform imageRect;

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
            frameImage = GetComponent<Image>();
            imageRect = frameImage.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
        }

        public void SetFrame(float width, float height)
        {
            AnchorPresets.SetAnchorPreset(imageRect, AnchorPresets.MiddleCenter);
            imageRect.sizeDelta = new Vector2(width, height);
            imageRect.localPosition = Vector2.zero;
        }

        public void SetFrameSprite(Sprite sprite)
        {
            frameImage.sprite = sprite;
        }
    }
}