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
        public GameObject Canvases;

        public GameObject SkullSecondTrap;
        public GameObject terrapupaController;
        public GameObject terrapupaMapObjectController;

        public GameObject[] uiPrefabs;

        public GameObject stonePillarPuzzle;
        public int curStage = 1;

        private void Awake()
        {
            MangerControllers.ClearAction(ManagerType.Input);
            MangerControllers.ClearAction(ManagerType.Data);

            Init();
        }

        protected override void Start()
        {
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

            if (Canvases != null)
            {
                foreach (Transform child in Canvases.transform)
                {
                    CheckTicket(child.gameObject);
                }
            }

            foreach (var ui in uiPrefabs)
            {
                Instantiate(ui, Canvases.transform);
            }

            CheckTicket(terrapupaController.gameObject);
            CheckTicket(terrapupaMapObjectController.gameObject);
            CheckTicket(SkullSecondTrap.gameObject);
            CheckTicket(stonePillarPuzzle.gameObject);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, "BGM3");
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Ambient, "cave 10");
        }

        private void Update()
        {
            //if(Input.GetKeyDown(KeyCode.Alpha0))
            //{
            //    SoundManager.Instance.PlaySound(SoundManager.SoundType.Ambient, "cave 10");
            //}
            //if(Input.GetKeyDown(KeyCode.Alpha9))
            //{
            //    SoundManager.Instance.StopAmbient("cave 10");
            //}
        }
    }
}