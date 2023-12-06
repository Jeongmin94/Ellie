using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.UI.Transform;
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

        public GroupType groupType;
        private UITransformData uiTransformData;

        public Sprite FrameImage
        {
            get => frameImage;
            set => frameImage = value;
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
        private readonly Queue<Rect> rectQueue = new Queue<Rect>();
        private readonly Queue<Vector2> scaleQueue = new Queue<Vector2>();

        public void InitFrameCanvas(UITransformData transformData)
        {
            uiTransformData = transformData;

#if UNITY_EDITOR
            uiTransformData.actionRect.Subscribe(OnRectChange);
            uiTransformData.actionScale.Subscribe(OnScaleChange);
#endif

            Init();
            Init(uiTransformData);
        }

        private void OnRectChange(Rect rect)
        {
            rectQueue.Enqueue(rect);
        }

        private void OnScaleChange(Vector2 scale)
        {
            scaleQueue.Enqueue(scale);
        }

        private void Init(UITransformData transformData)
        {
            Bind();
            InitObjects(transformData);
            BindEvent();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            framePanel = GetGameObject((int)GameObjects.FramePanel);
            panelRect = framePanel.GetComponent<RectTransform>();
        }

        private void InitObjects(UITransformData transformData)
        {
            SetRect(transformData.actionRect.Value);
        }

        private void SetRect(Rect rect)
        {
            AnchorPresets.SetAnchorPreset(panelRect, AnchorPresets.MiddleCenter);
            panelRect.sizeDelta = rect.GetSize();
            panelRect.localPosition = rect.ToCanvasPos();
        }

        private void SetScale(Vector2 scale)
        {
            panelRect.localScale = scale;
        }

        private void BindEvent()
        {
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (rectQueue.Any())
                SetRect(rectQueue.Dequeue());
            if (scaleQueue.Any())
                SetScale(scaleQueue.Dequeue());
#endif
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