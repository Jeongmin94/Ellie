using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory.DesctiptionPanel;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class Inventory : UIPopup
    {
        private enum GameObjects
        {
            DescriptionPanel,
            CategoryPanel,
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

        // Image
        private Image descImageArea;
        private Image inventorySlotArea;
        private Image equipSlotArea;

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

            descImageArea = GetImage((int)Images.DescriptionImageArea);
            inventorySlotArea = GetImage((int)Images.InventorySlotArea);
            equipSlotArea = GetImage((int)Images.EquipSlotArea);
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

            var slotAreaRect = inventorySlotArea.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(slotAreaRect, AnchorPresets.MiddleCenter);
            slotAreaRect.sizeDelta = InventoryConst.SlotAreaRect.GetSize();
            slotAreaRect.localPosition = InventoryConst.SlotAreaRect.ToCanvasPos();
            slotAreaRect.SetParent(categoryPanel.transform);

            var equipAreaRect = equipSlotArea.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(equipAreaRect, AnchorPresets.MiddleCenter);
            equipAreaRect.sizeDelta = InventoryConst.EquipSlotAreaRect.GetSize();
            equipAreaRect.localPosition = InventoryConst.EquipSlotAreaRect.ToCanvasPos();
            equipAreaRect.SetParent(categoryPanel.transform);
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
    }
}