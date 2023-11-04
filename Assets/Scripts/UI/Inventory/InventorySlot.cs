using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlot : UIBase
    {
        private enum Images
        {
            ItemImage
        }

        private enum Texts
        {
            ItemCount
        }

        private RectTransform rect;
        private Image itemImage;
        private TextMeshProUGUI itemCount;

        // !TODO: 스크립터블 오브젝트 아이템으로 현재 슬롯에 위치한 아이템 확인하기
        // 슬롯에 필요한 것들
        //  - 아이템 개수
        //  - 아이템 이미지

        private readonly int fontSize = 28;
        private readonly float lineHeight = 25.0f;

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

            itemImage = GetImage((int)Images.ItemImage);
            itemCount = GetText((int)Texts.ItemCount);
        }

        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.SlotRect.GetSize();
            rect.localPosition = InventoryConst.SlotRect.ToCanvasPos();

            itemCount.lineSpacing = lineHeight;
            itemCount.fontSize = fontSize;
            itemCount.color = Color.white;
            itemCount.alignment = TextAlignmentOptions.MidlineRight;
            itemCount.text = string.Empty;
        }

        // !TODO: 스위칭 전용 슬롯 추가 필요
        //      - 스위칭 슬롯 -> 아이템 슬롯: 이동됨, 스위칭 슬롯에서 빠짐
        //      - 아이템 슬롯 -> 스위칭 슬롯: 복사됨, 아이템 슬롯에는 그대로 있음
        //      - 스위칭 슬롯에 등록되면 static ui에 등록된 아이템 표시되어야 함
    }
}