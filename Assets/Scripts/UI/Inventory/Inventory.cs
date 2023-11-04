using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.UI.Inventory
{
    public delegate void ToggleChangeHandler(ToggleChangeInfo changeInfo);

    public class Inventory : UIPopup
    {
        private enum GameObjects
        {
            DescriptionPanel,
            CategoryPanel,
            GoldAndStonePiecePanel,
            CategoryButtonPanel // toggle group
        }

        private enum Images
        {
            DescriptionImageArea,
            InventorySlotArea,
            EquipSlotArea,
        }

        private void Awake()
        {
            Init();
        }

        // GameObject
        private GameObject descriptionPanel;
        private GameObject categoryPanel;
        private GameObject goldAndStonePiecePanel;
        private GameObject categoryButtonPanel;

        // Image
        private Image descImageArea;
        private Image inventorySlotArea;
        private Image equipSlotArea;

        // Category Grid
        private GridLayoutGroup slotGrid;
        private GridLayoutGroup equipGrid;

        // Category Toggle Button
        private CategoryButtonPanel buttonPanel;

        [Tooltip("Slot Area Grid")]
        [SerializeField]
        private int row = 3;

        [SerializeField] private int col = 8;
        [SerializeField] private int padding = 1;
        [SerializeField] private int spacing = 2;

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            descriptionPanel = GetGameObject((int)GameObjects.DescriptionPanel);
            categoryPanel = GetGameObject((int)GameObjects.CategoryPanel);
            goldAndStonePiecePanel = GetGameObject((int)GameObjects.GoldAndStonePiecePanel);
            categoryButtonPanel = GetGameObject((int)GameObjects.CategoryButtonPanel);

            descImageArea = GetImage((int)Images.DescriptionImageArea);
            inventorySlotArea = GetImage((int)Images.InventorySlotArea);
            equipSlotArea = GetImage((int)Images.EquipSlotArea);

            slotGrid = inventorySlotArea.gameObject.GetOrAddComponent<GridLayoutGroup>();
            equipGrid = equipSlotArea.gameObject.GetOrAddComponent<GridLayoutGroup>();

            buttonPanel = categoryButtonPanel.GetOrAddComponent<CategoryButtonPanel>();
        }

        private void InitObjects()
        {
            var descRect = descriptionPanel.GetComponent<RectTransform>();
            var ctgyRect = categoryPanel.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(descRect, AnchorPresets.MiddleCenter);
            AnchorPresets.SetAnchorPreset(ctgyRect, AnchorPresets.MiddleCenter);
            descRect.sizeDelta = InventoryConst.DescRect.GetSize();
            ctgyRect.sizeDelta = InventoryConst.CtgyRect.GetSize();
            descRect.localPosition = InventoryConst.DescRect.ToCanvasPos();
            ctgyRect.localPosition = InventoryConst.CtgyRect.ToCanvasPos();

            var descImageRect = descImageArea.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(descImageRect, AnchorPresets.MiddleCenter);
            descImageRect.sizeDelta = InventoryConst.DescImageRect.GetSize();
            descImageRect.localPosition = InventoryConst.DescImageRect.ToCanvasPos();
            descImageRect.SetParent(descriptionPanel.transform);

            InitSlotArea();

            InitEquipArea();

            var goldRect = goldAndStonePiecePanel.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(goldRect, AnchorPresets.MiddleCenter);
            goldRect.sizeDelta = InventoryConst.GoldRect.GetSize();
            goldRect.localPosition = InventoryConst.GoldRect.ToCanvasPos();
            goldRect.SetParent(categoryPanel.transform);

            var goldArea = UIManager.Instance.MakeSubItem<ImageAndTextArea>(transform, UIManager.ImageAndTextArea);
            goldArea.Rect.sizeDelta = Vector2.zero;
            goldArea.Rect.localPosition = Vector3.zero;

            SetValues(goldArea.Image.GetComponent<RectTransform>(), goldArea.transform, AnchorPresets.MiddleCenter, InventoryConst.GoldAreaRect);
            SetValues(goldArea.Text.GetComponent<RectTransform>(), goldArea.transform, AnchorPresets.MiddleCenter, InventoryConst.GoldAreaCountRect);

            goldArea.transform.SetParent(goldRect.transform);

            var stoneArea = UIManager.Instance.MakeSubItem<ImageAndTextArea>(transform, UIManager.ImageAndTextArea);
            stoneArea.Rect.sizeDelta = Vector2.zero;
            stoneArea.Rect.localPosition = Vector3.zero;

            SetValues(stoneArea.Image.GetComponent<RectTransform>(), stoneArea.transform, AnchorPresets.MiddleCenter, InventoryConst.StonePieceAreaRect);
            SetValues(stoneArea.Text.GetComponent<RectTransform>(), stoneArea.transform, AnchorPresets.MiddleCenter, InventoryConst.StonePieceAreaCountRect);

            stoneArea.transform.SetParent(goldRect.transform);
        }


        private void Start()
        {
            var descName = UIManager.Instance.MakeSubItem<DescriptionNamePanel>(transform, UIManager.DescriptionNamePanel);
            descName.transform.SetParent(descriptionPanel.transform);

            var descText = UIManager.Instance.MakeSubItem<DescriptionTextPanel>(transform, UIManager.DescriptionTextPanel);
            descText.transform.SetParent(descriptionPanel.transform);

            var slot = UIManager.Instance.MakeSubItem<InventorySlot>(transform, UIManager.InventorySlot);
            slot.transform.SetParent(descriptionPanel.transform);
        }

        private void InitSlotArea()
        {
            var slotAreaRect = inventorySlotArea.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(slotAreaRect, AnchorPresets.MiddleCenter);
            slotAreaRect.sizeDelta = InventoryConst.SlotAreaRect.GetSize();
            slotAreaRect.localPosition = InventoryConst.SlotAreaRect.ToCanvasPos();
            slotAreaRect.SetParent(categoryPanel.transform);

            float width = slotAreaRect.rect.width;
            float height = slotAreaRect.rect.height;

            var paddingOffset = new RectOffset();
            paddingOffset.SetAllPadding(padding);
            var spacingOffset = new Vector2(spacing, spacing);

            var w = width - paddingOffset.left * 2 - (col - 1) * spacingOffset.x;
            var slotW = w / col;
            var h = height - paddingOffset.top * 2 - (row - 1) * spacingOffset.y;
            var slotH = h / row;

            float len = Mathf.Min(slotH, slotW);
            slotGrid.spacing = spacingOffset;
            slotGrid.padding = paddingOffset;
            slotGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            slotGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
            slotGrid.childAlignment = TextAnchor.MiddleCenter;
            slotGrid.cellSize = new Vector2(len, len);

            for (int i = 0; i < row * col; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<InventorySlot>(slotAreaRect, UIManager.InventorySlot);
            }
        }

        private void InitEquipArea()
        {
            var equipAreaRect = equipSlotArea.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(equipAreaRect, AnchorPresets.MiddleCenter);
            equipAreaRect.sizeDelta = InventoryConst.EquipSlotAreaRect.GetSize();
            equipAreaRect.localPosition = InventoryConst.EquipSlotAreaRect.ToCanvasPos();
            equipAreaRect.SetParent(categoryPanel.transform);

            float width = equipAreaRect.rect.width;
            float height = equipAreaRect.rect.height;

            var paddingOffset = new RectOffset();
            paddingOffset.SetAllPadding(padding);
            var spacingOffset = new Vector2(spacing, spacing);

            int column = 5;
            var w = width - paddingOffset.left * 2 - (column - 1) * spacingOffset.x;
            var slotW = w / column;

            equipGrid.spacing = spacingOffset;
            equipGrid.padding = paddingOffset;
            equipGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            equipGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
            equipGrid.childAlignment = TextAnchor.MiddleCenter;
            equipGrid.cellSize = new Vector2(slotW, slotW);

            for (int i = 0; i < column; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<InventorySlot>(equipAreaRect, UIManager.InventorySlot);
            }
        }
    }
}