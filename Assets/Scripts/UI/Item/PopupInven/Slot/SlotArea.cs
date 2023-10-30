using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using Resolution = Assets.Scripts.Managers.Resolution;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public class SlotArea : UIBase
    {
        private enum GameObjects
        {
            SlotArea,
        }

        // slot grid configurations
        public int padding = 5;
        public int spacing = 5;
        public int row = 3;
        public int col = 8;

        private GameObject slotArea;
        private GameObject switchingArea;

        private RectTransform slotRect;
        private GridLayoutGroup slotGrid;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            GameManager.Instance.changeResolutionAction -= OnChangeResolution;
            GameManager.Instance.changeResolutionAction += OnChangeResolution;
        }

        protected override void Init()
        {
            Bind<GameObject>(typeof(GameObjects));

            slotArea = GetGameObject((int)GameObjects.SlotArea);
            slotRect = slotArea.GetOrAddComponent<RectTransform>();
            slotGrid = slotArea.GetOrAddComponent<GridLayoutGroup>();
        }

        public void SetAnchorPreset(AnchorPreset preset)
        {
            slotRect.anchorMin = preset.AnchorMin;
            slotRect.anchorMax = preset.AnchorMax;
        }

        public void Reset()
        {
            slotRect.sizeDelta = Vector2.zero;
            slotRect.transform.localPosition = Vector2.zero;
        }

        public void InitSlotPanel()
        {
            OnChangeResolution(GameManager.Instance.resolution);
            CreateSlot(row * col);

            // for (int i = 0; i < row * col; i++)
            // {
            //     var slot = UIManager.Instance.MakeSubItem<Slot>(slotRect, UIManager.UISlot);
            //
            //     var item = UIManager.Instance.MakeSubItem<SlotItem>(slot.ItemPosition, UIManager.UISlotItem);
            //     var itemRect = item.GetComponent<RectTransform>();
            //
            //     itemRect.anchorMin = AnchorPresets.StretchAll.AnchorMin;
            //     itemRect.anchorMax = AnchorPresets.StretchAll.AnchorMax;
            //     itemRect.sizeDelta = Vector2.zero;
            // }
        }

        private void CreateSlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                UIManager.Instance.MakeSubItem<Slot>(slotRect, UIManager.UISlot);
            }
        }

        private void OnChangeResolution(Resolution resolution)
        {
            var paddingOffset = new RectOffset();
            paddingOffset.SetAllPadding(padding);
            var spacingOffset = new Vector2(this.spacing, spacing);

            float width = slotRect.rect.width - paddingOffset.left * 2 - (col - 1) * spacingOffset.x;
            float slotWidth = width / col;

            float height = slotRect.rect.height - paddingOffset.top * 2 - (row - 1) * spacingOffset.y;
            float slotHeight = height / row;

            slotGrid.spacing = spacingOffset;
            slotGrid.padding = paddingOffset;
            slotGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            slotGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
            slotGrid.childAlignment = TextAnchor.MiddleCenter;
            slotGrid.cellSize = new Vector2(slotWidth, slotHeight);
        }
    }
}