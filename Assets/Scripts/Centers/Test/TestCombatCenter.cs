using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Player;
using Centers;
<<<<<<< HEAD
using Assets.Scripts.Monsters;
using UnityEngine;
using System.Collections;

=======
using UnityEngine;

>>>>>>> e309447d2abc77fd22f38d25cf7ce781f5eb286b
namespace Assets.Scripts.Centers.Test
{
    public class TestCombatCenter : BaseCenter
    {
        public GameObject player;
        public TestAttacker attacker;
<<<<<<< HEAD
        public MonsterController[] monsters;
=======
        public StoneHatchery hatchery;
        public Ore[] ores;
>>>>>>> e309447d2abc77fd22f38d25cf7ce781f5eb286b
        
       
        private void Awake()
        {
            Init();
        }

        protected override void Start()
<<<<<<< HEAD
        {
            CheckTicket(player.gameObject);
            CheckTicket(attacker.gameObject);
            foreach(MonsterController monster in monsters)
            {
                CheckTicket(monster.gameObject);
            }
        }


=======
        {
            CheckTicket(player);
            CheckTicket(attacker.gameObject);
            CheckTicket(hatchery.gameObject);
            foreach(Ore ore in ores)
            {
                Debug.Log($"{ore.name} checked");
                CheckTicket(ore.gameObject);
            }
        }
>>>>>>> e309447d2abc77fd22f38d25cf7ce781f5eb286b
    }
}
