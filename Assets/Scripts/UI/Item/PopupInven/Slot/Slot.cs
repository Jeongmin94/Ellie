using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public struct SlotInfo
    {
        public readonly Slot slot;

        private SlotInfo(Slot slot)
        {
            this.slot = slot;
        }

        public static SlotInfo Of(Slot slot)
        {
            return new SlotInfo(slot);
        }
    }

    public class Slot : UIBase, ISettable
    {
        private enum GameObjects
        {
            ItemPosition
        }

        public Transform ItemPosition => itemPosition.transform;
        private GameObject itemPosition;
        public bool IsUsed { get; set; }

        public int Index { get; set; }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind<GameObject>(typeof(GameObjects));

            itemPosition = GetGameObject((int)GameObjects.ItemPosition);

            gameObject.BindEvent(OnDropHandler, UIEvent.Drop);
        }

        // !TODO: 스위칭 슬롯인 경우 아이템 복사해서 등록해야 함
        private void OnDropHandler(PointerEventData data)
        {
            var droppedItem = data.pointerDrag;
            var draggable = droppedItem.GetComponent<IDraggable>();

            if (draggable == null)
            {
                return;
            }

            // 슬롯이 위치 이동인지 아니면 핫스왑 등록인지 여부에 따라서 등록 설정
            IsUsed = true;
            draggable.SetSlotInfo(SlotInfo.Of(this));
        }
    }
}