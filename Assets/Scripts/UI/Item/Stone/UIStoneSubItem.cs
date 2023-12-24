using TMPro;
using UI.Framework;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Item.Stone
{
    public class UIStoneSubItem : UIBase
    {
        // stone name, image, count etc ...
        private TextMeshProUGUI text;

        public Vector3 PrevPosition { get; set; }
        public Vector3 PrevScale { get; set; }
        public int ItemIdx { get; set; }

        public Image ItemImage { get; private set; }

        public string ItemText
        {
            get => text.text;
            set => text.text = value;
        }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            text = gameObject.FindChild<TextMeshProUGUI>();
            ItemImage = gameObject.FindChild<Image>();
        }
    }
}