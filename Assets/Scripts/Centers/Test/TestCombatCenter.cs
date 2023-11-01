using Assets.Scripts.Player;
using Centers;
using Assets.Scripts.Monsters;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Centers.Test
{
    public class TestCombatCenter : BaseCenter
    {
        public PlayerStatus player;
        public TestAttacker attacker;
        public MonsterController[] monsters;
        
       
        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            CheckTicket(player.gameObject);
            CheckTicket(attacker.gameObject);
            foreach(MonsterController monster in monsters)
            {
                CheckTicket(monster.gameObject);
            }
        }


    }
}
