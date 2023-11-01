using Assets.Scripts.UI.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public enum ButtonType
    {
        Default, Pressed
    }
    
    public class ItemMenuButton : UIBase
    {
        private enum Texts
        {
            ButtonText
        }

        private TextMeshProUGUI buttonText;
        private Image buttonImage;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));

            buttonText = GetText((int)Texts.ButtonText);
            buttonImage = gameObject.GetComponent<Image>();
        }

        public void SetText(string text)
        {
            buttonText.text = text;
        }

        public void SetSprite(Sprite sprite)
        {
            buttonImage.sprite = sprite;
        }
    }
}