using Assets.Scripts.UI.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class ImageAndTextArea : UIBase
    {
        private enum Images
        {
            Image
        }

        private enum Texts
        {
            Text
        }

        public RectTransform Rect { get; private set; }
        public Image Image { get; private set; }
        public TextMeshProUGUI Text { get; private set; }

        private readonly Color fontColor = Color.white;
        private readonly int fontSize = 20;
        private readonly float lineHeight = 25;

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

            Rect = GetComponent<RectTransform>();
            Image = GetImage((int)Images.Image);
            Text = GetText((int)Texts.Text);
        }

        private void InitObjects()
        {
            Text.color = fontColor;
            Text.fontSize = fontSize;
            Text.lineSpacing = lineHeight;
            Text.alignment = TextAlignmentOptions.Bottom;
        }
    }
}