using Assets.Scripts.Managers;
using Channels.Type;
using Channels.UI;
using Data.GoogleSheet._4100Item;
using Managers.Data;
using Player;
using UI.Inventory.CategoryPanel;
using UI.Inventory.Slot;
using UnityEngine;

namespace Item
{
    public class BaseDropItem : MonoBehaviour, ILootable
    {
        public const string path = "Pastry, Sweets & Desserts/Materials";
        [SerializeField] private ItemData data;

        public void Visit(PlayerLooting player)
        {
            player.gameObject.GetComponentInParent<PlayerController>().TicketMachine
                .SendMessage(ChannelType.UI, GeneratateItemPayload());
        }

        public void SetItemData(int index)
        {
            Debug.Log(DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(index).index);
            data = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(index);
        }

        private UIPayload GeneratateItemPayload()
        {
            UIPayload payload = new();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Item;
            payload.itemData = data;

            Destroy(gameObject);
            return payload;
        }
    }
}