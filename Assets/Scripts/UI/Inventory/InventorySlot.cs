using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlot : UIBase
    {
        private RectTransform rect;
        
        // !TODO: 스크립터블 오브젝트 아이템으로 현재 슬롯에 위치한 아이템 확인하기

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
            
        }

        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.SlotRect.GetSize();
            rect.localPosition = InventoryConst.SlotRect.ToCanvasPos();
        }
        
        // !TODO: 스위칭 전용 슬롯 추가 필요
        //      - 스위칭 슬롯 -> 아이템 슬롯: 이동됨, 스위칭 슬롯에서 빠짐
        //      - 아이템 슬롯 -> 스위칭 슬롯: 복사됨, 아이템 슬롯에는 그대로 있음
        //      - 스위칭 슬롯에 등록되면 static ui에 등록된 아이템 표시되어야 함
    }
}