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
    public Ore[] ores;
    public GameObject monsters;

    public int curStage = 1;

    private void Start()
    {
        TicketManager.Instance.CheckTicket(player.gameObject);
        TicketManager.Instance.CheckTicket(player.GetComponent<PlayerInventory>().Inventory.gameObject);
        TicketManager.Instance.CheckTicket(hatchery.gameObject);
        foreach (Ore ore in ores)
        {
            Debug.Log($"{ore.name} checked");
            TicketManager.Instance.CheckTicket(ore.gameObject);
            ore.curStage = curStage;
        }
        if (monsters != null)
        {
            foreach (Transform child in monsters.transform)
            {
                TicketManager.Instance.CheckTicket(child.gameObject);
            }
        }
    }
}
