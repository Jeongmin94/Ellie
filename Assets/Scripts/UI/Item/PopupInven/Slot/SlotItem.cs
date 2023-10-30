using Assets.Scripts.UI.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public class SlotItem : UIBase
    {
        private enum Images
        {
            ItemImage
        }

        private enum Texts
        {
            ItemText
        }

        private Image itemImage;
        private TextMeshProUGUI itemText;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            // !TODO: 슬롯 아이템 구현
            // 드래그 앤 드롭으로 슬롯 이동가능

            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            itemImage = GetImage((int)Images.ItemImage);
            itemText = GetText((int)Texts.ItemText);
        }
    }
}