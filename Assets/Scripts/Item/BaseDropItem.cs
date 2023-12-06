using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseDropItem : MonoBehaviour, ILootable
    {
        public const string path = "Pastry, Sweets & Desserts/Materials";
        [SerializeField] private ItemData data = null;

        public void Visit(PlayerLooting player)
        {
            player.gameObject.GetComponentInParent<PlayerController>().TicketMachine.SendMessage(ChannelType.UI, GeneratateItemPayload());
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
            payload.slotAreaType = UI.Inventory.SlotAreaType.Item;
            payload.groupType = UI.Inventory.GroupType.Item;
            payload.itemData = data;

            Destroy(gameObject);
            return payload;
        }
    }
}