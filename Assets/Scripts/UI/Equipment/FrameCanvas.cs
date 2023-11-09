using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI.Equipment
{
    public class FrameCanvas : UIStatic
    {
        public static readonly string Path = "Equipment/FrameCanvas";

        private enum GameObjects
        {
            FramePanel
        }

        [SerializeField] private Sprite frameImage;
        [SerializeField] private float frameWidth = 86.0f;
        [SerializeField] private float frameHeight = 86.0f;
        [SerializeField] private float spacing = 1.0f;
        [SerializeField] private Rect framePanelRect;

        public GroupType groupType;

        public Sprite FrameImage
        {
            get => frameImage;
            set => frameImage = value;
        }

        public Rect FramePanelRect
        {
            get { return framePanelRect; }
            set { framePanelRect = value; }
        }

        public float FrameWidth
        {
            get => frameWidth;
            set => frameWidth = value;
        }

        public float FrameHeight
        {
            get => frameHeight;
            set => frameHeight = value;
        }

        // frame
        private GameObject framePanel;
        private RectTransform panelRect;

        private readonly List<EquipmentFrame> frames = new List<EquipmentFrame>();

        public void InitFrameCanvas()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
            BindEvent();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            framePanel = GetGameObject((int)GameObjects.FramePanel);
            panelRect = framePanel.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(panelRect, AnchorPresets.MiddleCenter);
            panelRect.sizeDelta = framePanelRect.GetSize();
            panelRect.localPosition = framePanelRect.ToCanvasPos();
        }

        private void BindEvent()
        {
        }

        public void InitFrame(Vector2[] directions)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                var frame = UIManager.Instance.MakeSubItem<EquipmentFrame>(panelRect, EquipmentFrame.Path);
                frame.name = $"{frame.name}#{i}";
                frame.SetFrame(frameWidth, frameHeight);
                frame.SetFrameImage(frameImage);
                frame.transform.Translate(directions[i]);

                frames.Add(frame);
            }
        }

        public Transform GetPosition(int index) => frames[index].ImageRect;

        public void EquipItem(InventorySlot slot, BaseSlotItem baseSlotItem)
        {
        }

        // 아이템 장착
        public void EquipItem(BaseSlotItem baseItem)
        {
            var slot = baseItem.GetSlot();
            int slotIdx = slot.Index;
            if (slotIdx < 0 || slotIdx >= frames.Count)
            {
                Debug.LogWarning($"Frames index out of range({frames.Count}), slot index: {slot.Index}");
                return;
            }

            var frame = frames[slotIdx];
            frame.TurnOffFrameItem();
            {
                frame.SetItemImage(baseItem.SlotItemData.ItemSprite);
                frame.SetItemText(baseItem.SlotItemData.itemCount.Value.ToString());
            }
            frame.TurnOnFrameItem();
        }

        // 아이템 장착 해제
        public void UnEquipItem(BaseSlotItem baseItem)
        {
            var slot = baseItem.GetSlot();
            int slotIdx = slot.Index;
            if (slotIdx < 0 || slotIdx >= frames.Count)
            {
                Debug.LogWarning($"Frames index out of range({frames.Count}), slot index: {slot.Index}");
                return;
            }

            var frame = frames[slotIdx];
            frame.TurnOffFrameItem();
        }
    }
}