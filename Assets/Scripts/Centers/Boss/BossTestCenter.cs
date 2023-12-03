using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

public class BossTestCenter : MonoBehaviour
{
    public GameObject player;
    public StoneHatchery hatchery;
    public Inventory inventory;

    private void Start()
    {
        TicketManager.Instance.CheckTicket(player.gameObject);
        TicketManager.Instance.CheckTicket(player.GetComponent<PlayerInventory>().Inventory.gameObject);
        TicketManager.Instance.CheckTicket(hatchery.gameObject);
    }
}
