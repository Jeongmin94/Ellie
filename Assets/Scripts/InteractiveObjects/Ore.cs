using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Dialog;
using Channels.Stone;
using Channels.Type;
using Channels.UI;
using Data.GoogleSheet;
using Data.GoogleSheet._4000Stone;
using Managers.Data;
using Managers.Sound;
using Outline;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace InteractiveObjects
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
        public OreData data;
        public Tier tier;

        public int curStage;
        public int hardness;
        public int maxHp;
        public int hp;

        public float regenerationTime = 4f;
        public bool canMine;
        private int curHpInterval;

        //npc 이벤트
        private Action firstMineAction;
        private bool isFirstMine = true;
        private Transform oreBody;

        //체력 4분할
        private List<int> quateredHP;
        private Transform stoneSpawnPos;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            InitTicketMachine();
        }

        private void Start()
        {
            StartCoroutine(InitOre());
            oreBody = transform.GetChild(0);
            stoneSpawnPos = transform.GetChild(1);
            canMine = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().SetCurOre(null);
            }
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

        public void Smith(int damage)
        {
            hp -= damage;
            var idx = 0;
            //1/4만큼 체력이 감소할 때 마다 아이템 뱉기
            for (var i = 0; i < 3; i++)
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
            var temp = maxHp;
            //숫자 더해서 4의 배수로 만들기a
            while (temp % 4 != 0)
            {
                temp++;
            }

            var segment = temp / 4;
            quateredHP = new List<int>();
            //4분할된 구간을 만들기
            for (var i = 0; i < 4; i++)
            {
                quateredHP.Add(maxHp - segment * i);
            }

            quateredHP.Add(0);
            quateredHP.Reverse();
        }

        private void DropStone(List<(int, float)> dropItemList)
        {
            var rand = Random.Range(0f, 1.0f);
            var accChance = 0f;
            foreach (var item in dropItemList)
            {
                //special 드롭테이블인지 먼저 확인
                var dropTableData =
                    DataManager.Instance.GetIndexData<DropTableData, DropTableDataParsingInfo>(item.Item1);
                
                if (dropTableData.isSpecialDropTable)
                {
                    //special 드롭테이블이라면
                    var dropDataTuple = dropTableData.specialDropDataTuple;
                    for (int i = 0; i < dropDataTuple.Item2; i++)
                    {
                        MineStone(dropDataTuple.Item1);
                    }
                }
                else
                {
                    //아니라면
                    var dropDataList = dropTableData.stoneDropDataList;
                    accChance += item.Item2;
                    if (rand <= accChance)
                    {
                        foreach (var dropData in dropDataList)
                        {
                            for (var i = 0; i < dropData.Item2; i++)
                            {
                                var stoneList = DataManager.Instance.GetData<StoneDataParsingInfo>().stones
                                    .Where(obj => obj.tier == dropData.Item1 && obj.appearanceStage <= curStage).ToList();
                                if (stoneList.Count <= 0)
                                {
                                    continue;
                                }

                                var randIndex = Random.Range(0, stoneList.Count);
                                MineStone(stoneList[randIndex].index);
                            }
                        }
                        break;
                    }
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
                StoneIdx = stoneIdx
            };
            Debug.Log("Mine Stone : " + stoneIdx);
            ticketMachine.SendMessage(ChannelType.Stone, payload);
        }

        private Vector3 GetRandVector()
        {
            Vector3 vec = new(Random.Range(-1.0f, 1.0f), 0.5f, 0);
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
            var player = obj.GetComponent<PlayerController>();
            if (!player.IsPickaxeAvailable)
            {
                var payload = DialogPayload.Play("곡괭이가 있으면 돌멩이를 채광할 수 있을 것 같다..!");
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