using Assets.Scripts.Channels.Item;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class StoneHatchery : MonoBehaviour
    {
        private TicketMachine ticketMachine;
        private Pool stonePool;
        [SerializeField] Mesh[] stoneMeshes;

        [SerializeField] private GameObject stone;
        private const int initialPoolSize = 10;
        private void Awake()
        {
            SetTicketMachine();
            InitStonePool();
        }
        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Stone);
            //ticketMachine.GetTicket(ChannelType.Combat).SubscribeNotifyAction(ReleaseStoneEvent);
            ticketMachine.RegisterObserver(ChannelType.Stone, StoneEvent);
        }
        private void InitStonePool()
        {
            //돌맹이 일정량만큼 풀에서 받아서 걔네 티켓 만들어주고 해처리의 공격함수 구독
            stonePool = PoolManager.Instance.CreatePool(stone, initialPoolSize);
        }

        public void Attack(CombatPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public Poolable GetStone(int stoneIdx)
        {
            Poolable obj = stonePool.Pop();
            obj.GetComponent<BaseStone>().data = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(stoneIdx);
            obj.GetComponent<BaseStone>().hatchery = this;
            int idx = Random.Range(0, stoneMeshes.Length);
            obj.gameObject.GetComponent<MeshFilter>().mesh = stoneMeshes[idx];
            obj.gameObject.GetComponent<MeshCollider>().sharedMesh = stoneMeshes[idx];
            return obj;
        }

        public void CollectStone(BaseStone stone)
        {
            stonePool.Push(stone);
        }

        public void StoneEvent(IBaseEventPayload payload)
        {
            StoneEventPayload itemPayload = payload as StoneEventPayload;
            BaseStone stone = GetStone(itemPayload.StoneIdx) as BaseStone;
            Vector3 startPos = itemPayload.StoneSpawnPos;
            Vector3 direction = itemPayload.StoneDirection;
            Vector3 force = itemPayload.StoneForce;
            float strength = itemPayload.StoneStrength;
            if (itemPayload.Type == StoneEventType.RequestStone)
            {

                ReleaseStone(stone, startPos, direction, strength);
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
        }

        private void ReleaseStone(BaseStone stone, Vector3 startPos, Vector3 direction, float strength)
        {
            stone.SetPosition(startPos);
            stone.MoveStone(direction, strength);
        }
    }
}