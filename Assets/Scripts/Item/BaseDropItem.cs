using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Inventory;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Assets.Scripts.Item
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