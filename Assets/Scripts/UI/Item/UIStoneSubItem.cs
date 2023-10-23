using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
    public class UIStoneSubItem : UIBase
    {
        // stone name, image, count etc ...
        private TextMeshProUGUI text;
        private Image image;

        public Vector3 PrevPosition { get; set; }
        public Vector3 PrevScale { get; set; }
        public int ItemIdx { get; set; }

        public string ItemText
        {
            get { return text.text; }
            set { text.text = value; }
        }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            text = gameObject.FindChild<TextMeshProUGUI>();
            image = gameObject.FindChild<Image>();
        }
    }
}