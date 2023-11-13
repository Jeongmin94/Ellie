using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.Inventory.Test;
using Centers;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
{
    public class TestCenter : BaseCenter
    {
        public GameObject player;
        public TestAttacker attacker;
        public StoneHatchery hatchery;
        public InventoryTestClient inventoryClient;
        public Inventory inventory;
        public Ore[] ores;

        public int curStage = 1;
       
        private void Awake()
        {
            Init();
            inventory = UIManager.Instance.MakePopup<Inventory>(UIManager.Inventory);
        }

        protected override void Start()
        {
            CheckTicket(player.gameObject);
            CheckTicket(attacker.gameObject);

            //foreach (Transform childMonster in monsters)
            //{
            //    CheckTicket(childMonster.gameObject);
            //}

            CheckTicket(hatchery.gameObject);
            foreach (Ore ore in ores)
            {
                Debug.Log($"{ore.name} checked");
                CheckTicket(ore.gameObject);
                ore.curStage = curStage;
            }
            CheckTicket(inventoryClient.gameObject);
            CheckTicket(inventory.gameObject);
        }
    }
}
