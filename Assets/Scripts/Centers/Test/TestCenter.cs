using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.Inventory.Test;
using Centers;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
{
    public class TestCenter : BaseCenter
    {
        public GameObject player;
        public StoneHatchery hatchery;
        public Inventory inventory;
        public Ore[] ores;
        public GameObject monsters;
        public GameObject DialogCanvas;
        public GameObject SimpleDialogCanvas;

        public GameObject SkullSecondTrap;
        public TestAttacker attacker;

        public int curStage = 1;

        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            CheckTicket(player.gameObject);
            CheckTicket(player.GetComponent<PlayerInventory>().Inventory.gameObject);
            CheckTicket(hatchery.gameObject);
            //CheckTicket(attacker.gameObject);
            foreach (Ore ore in ores)
            {
                Debug.Log($"{ore.name} checked");
                CheckTicket(ore.gameObject);
                ore.curStage = curStage;
            }

            if (monsters != null)
            {
                foreach (Transform child in monsters.transform)
                {
                    CheckTicket(child.gameObject);
                }
            }
            CheckTicket(DialogCanvas.gameObject);
            CheckTicket(SimpleDialogCanvas.gameObject);
            CheckTicket(SkullSecondTrap.gameObject);
        }
    }
}