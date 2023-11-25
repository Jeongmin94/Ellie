using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Player;
using Channels.Type;
using Channels.UI;
using UnityEngine;
using Assets.Scripts.UI.Inventory;

public class DropItem : MonoBehaviour, ILootable
{
    public float dropChance;

    public void Visit(PlayerLooting player)
    {
        Debug.Log("Player Loot : " + name);
        //player.gameObject.GetComponentInParent<PlayerController>().TicketMachine.SendMessage(ChannelType.UI )
    }

    //private UIPayload GenerateConsumptionItem()
    //{
    //    UIPayload payload = new UIPayload();
    //    payload.uiType = UIType.Notify;
    //    payload.actionType = ActionType.AddSlotItem;
    //    payload.slotAreaType = SlotAreaType.Item;
    //    payload.groupType = GroupType.Consumption;
    //    //payload.itemData = 
    //}
}
