using Assets.Scripts.Channels.Item;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Channels.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Outline;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public class Ore : InteractiveObject
    {
        public enum Tier
        {
            Tier3 = 9100,
            Tier2,
            Tier1,
            Boom,
            Ice,
            Dark,
            Portal,
            Normal
        }

        [SerializeField] private Renderer renderer;

        public InteractiveType interactiveType = InteractiveType.Mining;
        private Transform oreBody;
        private Transform stoneSpawnPos;
        public OreData data;
        public Tier tier;

        public int curStage;
        public int hardness;
        public int maxHp;
        public int hp;

        private TicketMachine ticketMachine;

        //체력 4분할
        private List<int> quateredHP;
        private int curHpInterval;

        public float regenerationTime = 4f;
        public bool canMine;

        //npc 이벤트
        private Action firstMineAction;
        private bool isFirstMine = true;

        private void Awake()
        {
            InitTicketMachine();
        }

        public override InteractiveType GetInteractiveType()
        {
            return interactiveType;
        }

        public override OutlineType GetOutlineType()
        {
            return OutlineType.InteractiveOutline;
        }

        public override Renderer GetRenderer()
        {
            return renderer;
        }

        private void Start()
        {
            StartCoroutine(InitOre());
            oreBody = transform.GetChild(0);
            stoneSpawnPos = transform.GetChild(1);
            canMine = true;
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Stone, ChannelType.Dialog);
        }

        private IEnumerator InitOre()
        {
            yield return DataManager.Instance.CheckIsParseDone();

            data = DataManager.Instance.GetIndexData<OreData, OreDataParsingInfo>((int)tier);
            hardness = data.hardness;
            maxHp = data.HP;
            hp = maxHp;
            curHpInterval = 3;
            SetQuateredHP();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(null);
            }
        }

        public void Smith(int damage)
        {
            hp -= damage;
            int idx = 0;
            //1/4만큼 체력이 감소할 때 마다 아이템 뱉기
            for (int i = 0; i < 3; i++)
            {
                if (hp >= quateredHP[i] && hp <= quateredHP[i + 1] && curHpInterval != i)
                {
                    //구간을 두 번 지나쳤을 때 두 번 드랍시키기 위함
                    idx = curHpInterval - i;
                    curHpInterval = i;
                }
            }

            while (idx > 0)
            {
                Debug.Log("during mining");
                DropStone(data.whileMiningDropItemList);
                idx--;
                if (isFirstMine)
                {
                    Publish();
                    isFirstMine = false;
                }
            }

            if (hp <= 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "rock_destruction2", transform.position);
                canMine = false;
                //아이템 뱉기
                DropStone(data.miningEndDropItemList);
                StartCoroutine(Regenerate());
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "rock_destruction1", transform.position);
            }
        }

        private void SetQuateredHP()
        {
            int temp = maxHp;
            //숫자 더해서 4의 배수로 만들기a
            while (temp % 4 != 0)
            {
                temp++;
            }

            int segment = temp / 4;
            quateredHP = new();
            //4분할된 구간을 만들기
            for (int i = 0; i < 4; i++)
            {
                quateredHP.Add(maxHp - segment * i);
            }

            quateredHP.Add(0);
            quateredHP.Reverse();
        }

        private void DropStone(List<(int, float)> dropItemList)
        {
            float rand = UnityEngine.Random.Range(0f, 1.0f);
            float accChance = 0f;
            foreach (var item in dropItemList)
            {
                //item.Item1 : 드롭테이블 인덱스
                //item.Item2 : 확률
                accChance += item.Item2;

                if (rand <= accChance)
                {
                    //드롭테이블 참조해서 돌맹이 생성
                    //인덱스로 드롭테이블 참조
                    List<(int, int)> dropDataList = DataManager.Instance.GetIndexData<DropTableData, DropTableDataParsingInfo>(item.Item1).stoneDropDataList;
                    foreach (var dropData in dropDataList)
                    {
                        for (int i = 0; i < dropData.Item2; i++)
                        {
                            //TODO : 돌맹이 데이터테이블에서 해당하는 티어의 돌맹이 랜덤하게 뽑아내기
                            //Item2 : 돌맹이 개수
                            //Item1 : 돌맹이 티어
                            //돌맹이 티어로 데이터풀에서 해당 티어에 맞는 돌맹이 && 현재 스테이지 이하의 돌 중 랜덤 생성하기
                            List<StoneData> tempStones = DataManager.Instance.GetData<StoneDataParsingInfo>().stones.Where(obj => obj.tier == dropData.Item1 && obj.appearanceStage <= curStage).ToList();
                            if (tempStones.Count <= 0) continue;
                            int randIndex = UnityEngine.Random.Range(0, tempStones.Count);
                            MineStone(tempStones[randIndex].index);
                        }
                    }

                    break;
                }
            }
        }

        private void MineStone(int stoneIdx)
        {
            StoneEventPayload payload = new()
            {
                Type = StoneEventType.MineStone,
                StoneSpawnPos = stoneSpawnPos.position,
                StoneForce = GetRandVector(),
                StoneIdx = stoneIdx,
            };
            Debug.Log("Mine Stone : " + stoneIdx.ToString());
            ticketMachine.SendMessage(ChannelType.Stone, payload);
        }

        private Vector3 GetRandVector()
        {
            Vector3 vec = new(UnityEngine.Random.Range(-1.0f, 1.0f), 0.5f, 0);
            return vec.normalized;
        }

        private IEnumerator Regenerate()
        {
            oreBody.gameObject.SetActive(false);
            yield return new WaitForSeconds(regenerationTime);
            canMine = true;
            oreBody.gameObject.SetActive(true);
            hp = maxHp;
        }

        public override void Interact(GameObject obj)
        {
            PlayerController player = obj.GetComponent<PlayerController>();
            if (!player.IsPickaxeAvailable)
            {
                DialogPayload payload = DialogPayload.Play("곡괭이가 있으면 돌멩이를 채광할 수 있을 것 같다..!");
                payload.canvasType = DialogCanvasType.Simple;
                ticketMachine.SendMessage(ChannelType.Dialog, payload);

                return;
            }

            player.SetCurOre(this);
        }

        public void SubscribeFirstMineAction(Action listener)
        {
            firstMineAction -= listener;
            firstMineAction += listener;
        }

        public void UnSubscribeFirstMineAction(Action listener)
        {
            firstMineAction -= listener;
        }

        private void Publish()
        {
            firstMineAction?.Invoke();
        }
    }
}