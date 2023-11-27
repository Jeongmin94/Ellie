using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Player;
using Channels.Type;
using Channels.UI;
using UnityEngine;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Managers;

public class DropItem : MonoBehaviour, ILootable
{
    public float dropChance;
    public int itemIndex;

    public void Visit(PlayerLooting player)
    {
        player.gameObject.GetComponentInParent<PlayerController>().TicketMachine.SendMessage(ChannelType.UI, GenerateConsumptionItem());
        Destroy(gameObject);
    }

    private UIPayload GenerateConsumptionItem()
    {
        UIPayload payload = new UIPayload();
        payload.uiType = UIType.Notify;
        payload.actionType = ActionType.AddSlotItem;
        payload.slotAreaType = SlotAreaType.Item;
        payload.groupType = GroupType.Item;
        payload.itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(itemIndex);

        return payload;
    }
}
