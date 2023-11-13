using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Player;
using Centers;
using Assets.Scripts.Monsters;
using UnityEngine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
{
    public class TestCombatCenter : BaseCenter
    {
        public GameObject player;
        public TestAttacker attacker;
        //public Transform monsters;
        public StoneHatchery hatchery;
        public Ore[] ores;

        public int curStage = 1;
       
        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            CheckTicket(player.gameObject);
            //CheckTicket(attacker.gameObject);

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
        }
    }
}
