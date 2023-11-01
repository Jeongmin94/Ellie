using Assets.Scripts.Item.Stone;
using Assets.Scripts.Player;
using Centers;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
{
    public class TestCombatCenter : BaseCenter
    {
        public GameObject player;
        public TestAttacker attacker;
        public StoneHatchery hatchery;
        
       
        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            CheckTicket(player);
            CheckTicket(attacker.gameObject);
            CheckTicket(hatchery.gameObject);
        }
    }
}
