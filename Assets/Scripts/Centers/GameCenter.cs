using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Singleton;
using Assets.Scripts.Player;
using Centers;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
{
    public class GameCenter : BaseCenter
    {
        public GameObject player;
        public StoneHatchery hatchery;
        public Ore[] ores;
        public GameObject monsterController;
        public GameObject monsters;
        public GameObject[] skullSecondTraps;
        public GameObject stonePillarPuzzle;
        public GameObject GuideColliders;
        public int curStage = 1;

        private void Awake()
        {
            Debug.Log($"%%%%% GameCenter Awake %%%%%");
            MangerControllers.ClearAction(ManagerType.Input);
            MangerControllers.ClearAction(ManagerType.Sound);
            MangerControllers.ClearAction(ManagerType.Particle);
            MangerControllers.ClearAction(ManagerType.EventBus);
            
            Init();
        }

        protected override void Start()
        {
            base.InitObjects();
            
            CheckTicket(player.gameObject);
            CheckTicket(player.GetComponent<PlayerInventory>().Inventory.gameObject);
            CheckTicket(hatchery.gameObject);
            foreach (Ore ore in ores)
            {
                CheckTicket(ore.gameObject);
                ore.curStage = curStage;
            }

            CheckTicket(monsterController.gameObject);
            foreach (Transform child in monsters.transform)
            {
                CheckTicket(child.gameObject);
            }
            foreach (var controller in controllerInstances)
            {
                CheckTicket(controller.gameObject);
            }
            foreach(var SkullSecondTrap in skullSecondTraps)
                CheckTicket(SkullSecondTrap.gameObject);
            CheckTicket(stonePillarPuzzle.gameObject);
            
            foreach (Transform guidecollider in GuideColliders.transform)
            {
                CheckTicket(guidecollider.gameObject);
            }
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, "BGM3");
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Ambient, "cave 10");
        }
    }
}