using Assets.Scripts.Managers;
using Channels.Combat;
using Channels.Components;
using Channels.Stone;
using Channels.Type;
using Channels.UI;
using Data.GoogleSheet._4000Stone;
using Managers.Data;
using Managers.Pool;
using UI.Inventory.Slot;
using UnityEngine;
using Utils;

namespace Item.Stone
{
    public class StoneHatchery : MonoBehaviour
    {
        private const int STONEIDXSTART = 4000;
        private const int initialPoolSize = 10;
        [SerializeField] private Mesh[] stoneMeshes;
        [SerializeField] private Material[] materials;
        [SerializeField] private BaseStoneEffect[] stoneEffects;
        [SerializeField] private GameObject stoneTrailTest;

        [SerializeField] private GameObject stone;
        private Pool stonePool;

        private Transform stoneRoot;
        private TicketMachine ticketMachine;

        private void Awake()
        {
            SetTicketMachine();
            InitStonePool();

            var root = new GameObject();
            root.name = "@Stone_Root";
            root.transform.SetParent(transform);
            stoneRoot = root.transform;
        }

        private void Start()
        {
            var stoneMaterialsPath = "Materials/StoneMaterials";
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
            var obj = stonePool.Pop(stoneRoot);
            obj.GetComponent<BaseStone>().data =
                DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(stoneIdx);
            if (obj.GetComponent<BaseStone>().data == null)
            {
                return null;
            }

            obj.GetComponent<BaseStone>().hatchery = this;
            var idx = Random.Range(0, stoneMeshes.Length);
            obj.gameObject.GetComponent<MeshFilter>().mesh = stoneMeshes[idx];
            obj.gameObject.GetComponent<MeshCollider>().sharedMesh = stoneMeshes[idx];
            var matIdx = obj.GetComponent<BaseStone>().data.index % STONEIDXSTART;
            obj.gameObject.GetComponent<MeshRenderer>().material = materials[matIdx];
            AddStoneEffect(obj, stoneIdx);

            return obj;
        }

        public void AddStoneEffect(Poolable obj, int stoneIdx)
        {
            var currentEffect = obj.GetComponent<BaseStoneEffect>();
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

            var prefab = obj.GetComponent<StonePrefab>();

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
            var itemPayload = payload as StoneEventPayload;
            var stone = GetStone(itemPayload.StoneIdx) as BaseStone;
            if (stone == null)
            {
                Debug.LogError("돌맹이 파싱 중...");
                return;
            }

            var startPos = itemPayload.StoneSpawnPos;
            var direction = itemPayload.StoneDirection;
            var force = itemPayload.StoneForce;
            var strength = itemPayload.StoneStrength;
            stone.GetComponent<StonePrefab>().StoneEffect.Type = itemPayload.Type;
            if (itemPayload.Type == StoneEventType.ShootStone)
            {
                ReleaseStone(stone, startPos, direction, strength);
                //UI 페이로드 작성
                UIPayload uIPayload = new()
                {
                    uiType = UIType.Notify,
                    actionType = ActionType.ConsumeSlotItem,
                    slotAreaType = SlotAreaType.Item,
                    itemData = stone.data
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
            stone.GetComponent<Rigidbody>().AddTorque(2f * Random.onUnitSphere);
        }

        private void ReleaseStone(BaseStone stone, Vector3 startPos, Vector3 direction, float strength)
        {
            stone.transform.position = startPos;

            stone.MoveStone(direction, strength);
            stone.GetComponent<Rigidbody>().AddTorque(2f * Random.onUnitSphere);
        }
    }
}