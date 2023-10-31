using Assets.Scripts.Item.Stone;
using Assets.Scripts.Player;
using Centers;

namespace Assets.Scripts.Centers.Test
{
    public class TestCombatCenter : BaseCenter
    {
        public PlayerStatus player;
        public TestAttacker attacker;
        public StoneHatchery hatchery;
        
       
        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            CheckTicket(player.gameObject);
            CheckTicket(attacker.gameObject);
            CheckTicket(hatchery.gameObject);
        }
    }
}
