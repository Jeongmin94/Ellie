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
        public GameObject SkullSecondTrap;
        public GameObject terrapupaController;
        public GameObject terrapupaMapObjectController;

        public GameObject stonePillarPuzzle;
        public int curStage = 1;

        private void Awake()
        {
            Debug.Log($"%%%%% GameCenter Awake %%%%%");
            MangerControllers.ClearAction(ManagerType.Input);
            MangerControllers.ClearAction(ManagerType.Data);
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
                Debug.Log($"{ore.name} checked");
                CheckTicket(ore.gameObject);
                ore.curStage = curStage;
            }

            CheckTicket(monsterController.gameObject);
            foreach (Transform child in monsters.transform)
            {
                CheckTicket(child.gameObject);
            }
            CheckTicket(terrapupaController.gameObject);
            CheckTicket(terrapupaMapObjectController.gameObject);
            CheckTicket(SkullSecondTrap.gameObject);
            CheckTicket(stonePillarPuzzle.gameObject);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, "BGM3");
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Ambient, "cave 10");
        }
    }
}