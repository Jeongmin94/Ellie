using Assets.Scripts.Channels.Item;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class StoneHatchery : MonoBehaviour
    {
        private const int STONEIDXSTART = 4000;
        private TicketMachine ticketMachine;
        private Pool stonePool;
        [SerializeField] Mesh[] stoneMeshes;
        [SerializeField] Material[] materials;
        [SerializeField] BaseStoneEffect[] stoneEffects;
        [SerializeField] GameObject stoneTrailTest;

        [SerializeField] private GameObject stone;
        private const int initialPoolSize = 10;

        private Transform stoneRoot;

        private void Awake()
        {
            SetTicketMachine();
            InitStonePool();
            
            GameObject root = new GameObject();
            root.name = "@Stone_Root";
            root.transform.SetParent(transform);
            stoneRoot = root.transform;
        }
        private void Start()
        {
            string stoneMaterialsPath = "Materials/StoneMaterials";
            materials = Resources.LoadAll<Material>(stoneMaterialsPath);
        }
        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Stone, ChannelType.UI, ChannelType.Portal);
            ticketMachine.RegisterObserver(ChannelType.Stone, StoneEvent);
        }

        private void InitStonePool()
        {
            //돌맹이 일정량만큼 풀에서 받아서 걔네 티켓 만들어주고 해처리의 공격함수 구독
            stonePool = PoolManager.Instance.CreatePool(stone, 10);
        }

        public void Attack(CombatPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public Poolable GetStone(int stoneIdx)
        {
            Poolable obj = stonePool.Pop(stoneRoot);
            obj.GetComponent<BaseStone>().data = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(stoneIdx);
            if (obj.GetComponent<BaseStone>().data == null)
            {
                return null;
            }
            obj.GetComponent<BaseStone>().hatchery = this;
            int idx = Random.Range(0, stoneMeshes.Length);
            obj.gameObject.GetComponent<MeshFilter>().mesh = stoneMeshes[idx];
            obj.gameObject.GetComponent<MeshCollider>().sharedMesh = stoneMeshes[idx];
            int matIdx = obj.GetComponent<BaseStone>().data.index % STONEIDXSTART;
            obj.gameObject.GetComponent<MeshRenderer>().material = materials[matIdx];
            AddStoneEffect(obj, stoneIdx);

            return obj;
        }

        public void AddStoneEffect(Poolable obj, int stoneIdx)
        {
            BaseStoneEffect currentEffect = obj.GetComponent<BaseStoneEffect>();
            if (currentEffect != null)
            {
                Destroy(currentEffect);
            }

            BaseStoneEffect effect;
            // 추후 enum + 데이터테이블 + 딕셔너리로 수정
            switch (stoneIdx)
            {
                case 4003:
                    effect = obj.gameObject.AddComponent<ExplosionStone>();
                    break;
                case 4005:
                    effect = obj.gameObject.AddComponent<IceStone>();
                    break;
                case 4019:
                    effect = obj.gameObject.AddComponent<PortalStone>();
                    break;
                case 4020:
                    effect = obj.gameObject.AddComponent<MagicStone>();
                    break;
                case 4021:
                    effect = obj.gameObject.AddComponent<GolemCoreStone>();
                    break;
                default:
                    effect = obj.gameObject.AddComponent<NormalStone>();
                    break;
            }
            StonePrefab prefab = obj.GetComponent<StonePrefab>();

            prefab.StoneEffect = effect;
            effect.InitData(prefab.data, ticketMachine);
            effect.SubscribeAction(prefab.OccurEffect);
        }

        public void CollectStone(BaseStone stone)
        {
            stonePool.Push(stone);
        }

        public void StoneEvent(IBaseEventPayload payload)
        {
            StoneEventPayload itemPayload = payload as StoneEventPayload;
            BaseStone stone = GetStone(itemPayload.StoneIdx) as BaseStone;
            if(stone == null)
            {
                Debug.LogError("돌맹이 파싱 중...");
                return;
            }
            Vector3 startPos = itemPayload.StoneSpawnPos;
            Vector3 direction = itemPayload.StoneDirection;
            Vector3 force = itemPayload.StoneForce;
            float strength = itemPayload.StoneStrength;
            stone.GetComponent<StonePrefab>().StoneEffect.Type = itemPayload.Type;
            if (itemPayload.Type == StoneEventType.ShootStone)
            {
                ReleaseStone(stone, startPos, direction, strength);
                //UI 페이로드 작성
                UIPayload uIPayload = new()
                {
                    uiType = UIType.Notify,
                    actionType = ActionType.ConsumeSlotItem,
                    slotAreaType = UI.Inventory.SlotAreaType.Item,
                    itemData = stone.data,
                };
                ticketMachine.SendMessage(ChannelType.UI, uIPayload);
            }
            else if (itemPayload.Type == StoneEventType.MineStone)
            {
                MineStone(stone, startPos, force);
            }
        }

        private void MineStone(BaseStone stone, Vector3 position, Vector3 force)
        {
            stone.transform.position = position;
            stone.GetComponent<Rigidbody>().AddForce(force * 4f, ForceMode.Impulse);
            stone.GetComponent<Rigidbody>().AddTorque(2f* Random.onUnitSphere);
        }

        private void ReleaseStone(BaseStone stone, Vector3 startPos, Vector3 direction, float strength)
        {
            stone.transform.position = startPos;

            stone.MoveStone(direction, strength);
            stone.GetComponent<Rigidbody>().AddTorque(2f*Random.onUnitSphere);
        }
    }
}