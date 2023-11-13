using Assets.Scripts.Managers;
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
        public SlotItem slotItem;


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

            // 슬롯에 똑같은 이름의 아이템이 있으면 수량 추가
            var si = droppedItem.GetComponent<SlotItem>();
            if (IsUsed)
            {
                if (slotItem.name.Equals(droppedItem.name))
                {
                    // !TODO: 카운트를 더한 아이템은 추후에 Destroy 처리해야 함
                    slotItem.AddCount(si.ItemCount);
                    ResourceManager.Instance.Destroy(si.gameObject);
                }
            }
            else
            {
                IsUsed = true;
                // 슬롯이 위치 이동인지 아니면 핫스왑 등록인지 여부에 따라서 등록 설정
                slotItem = si;
                // draggable.SetSlotInfo(SlotInfo.Of(this));
            }
        }
    }
}