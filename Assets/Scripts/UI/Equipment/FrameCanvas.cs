using System.Collections.Generic;
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

        public void InitFrame(Vector2[] directions, string uiName)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                var frame = UIManager.Instance.MakeSubItem<EquipmentFrame>(panelRect, uiName);
                frame.name = $"{frame.name}#{i}";
                frame.SetFrame(frameWidth, frameHeight);
                frame.SetFrameImage(frameImage);
                frame.transform.Translate(directions[i]);

                frames.Add(frame);
            }
        }

        public void RegisterObservers(List<InventorySlot> slots)
        {
            slots.ForEach(s => s.SubscribeEquipmentFrameAction(OnEquipmentFrameAction));
        }

        private void OnEquipmentFrameAction(InventoryEventPayload payload)
        {
            int slotIdx = payload.slot.Index;
            if (slotIdx < 0 || slotIdx >= frames.Count)
                return;

            EquipmentFrame frame = frames[slotIdx];
            if (payload.slot.SlotItemData == null)
            {
                frame.TurnOffFrameItem();
            }
            else
            {
                frame.SetItemImage(payload.slot.SlotItemData.ItemSprite);
                frame.SetItemText(payload.slot.SlotItemData.itemCount.Value.ToString());
                frame.TurnOnFrameItem();
            }
        }
    }
}