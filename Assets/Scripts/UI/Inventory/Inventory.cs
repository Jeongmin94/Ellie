using System;
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
            CategoryButtonPanel, // toggle group

            ItemSlots,
            EquipmentSlots
        }

        private enum Images
        {
            DescriptionImageArea,
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

        private GameObject itemSlots;
        private GameObject equipmentSlots;

        // Image
        private Image descImageArea;

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

            itemSlots = GetGameObject((int)GameObjects.ItemSlots);
            equipmentSlots = GetGameObject((int)GameObjects.EquipmentSlots);

            descImageArea = GetImage((int)Images.DescriptionImageArea);

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

            InitButtonPanel();

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

        private void InitButtonPanel()
        {
            InitItemArea();
            InitEquipmentArea();
        }

        private void InitItemArea()
        {
            var groupTypes = Enum.GetValues(typeof(GroupType));
            for (int i = 0; i < groupTypes.Length; i++)
            {
                var slotArea = UIManager.Instance.MakeSubItem<InventorySlotArea>(transform, InventorySlotArea.Path);

                slotArea.InitSlotRect(itemSlots.transform, InventoryConst.SlotAreaRect);
                slotArea.InitGridLayoutGroup(row, col, padding, spacing, GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.MiddleCenter);
                slotArea.SlotAreaType = SlotAreaType.Item;
                slotArea.MakeSlots();

                buttonPanel.AddSlot(SlotAreaType.Item, slotArea);
            }
        }

        private readonly int equipmentRow = 1;
        private readonly int[] equipmentCols = new[] { 4, 5, 2 };

        private void InitEquipmentArea()
        {
            var groupTypes = Enum.GetValues(typeof(GroupType));
            for (int i = 0; i < groupTypes.Length; i++)
            {
                var slotArea = UIManager.Instance.MakeSubItem<InventorySlotArea>(transform, InventorySlotArea.Path);
                slotArea.InitSlotRect(equipmentSlots.transform, InventoryConst.EquipSlotAreaRect);
                slotArea.InitGridLayoutGroup(equipmentRow, equipmentCols[i], padding, spacing, GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.MiddleCenter);
                slotArea.SlotAreaType = SlotAreaType.Equipment;
                slotArea.MakeSlots();

                buttonPanel.AddSlot(SlotAreaType.Equipment, slotArea);
            }
        }
    }
}