using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Channels.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Monster;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Utils;
using Centers;
using Channels.Components;
using Channels.Type;
using UnityEngine;

public class MonsterController : MonoBehaviour, IMonster
{
    private const int monsterDisableWait = 5;
    [SerializeField] GameObject monsters;
    private List<AbstractMonster> monster = new();
    TicketMachine ticketMachine;

    private void Awake()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.Monster, ChannelType.Stone);
        ticketMachine.RegisterObserver(ChannelType.Monster, MonsterDead);
        TicketManager.Instance.Ticket(ticketMachine);

        SetMonster();
    }

    private void SetMonster()
    {
        foreach (Transform child in monsters.transform)
        {
            monster.Add(child.GetComponent<AbstractMonster>());
        }
        foreach (AbstractMonster mon in monster)
        {
            Debug.Log(mon);
            mon.SetTicketMachine();
        }
    }

    public void MonsterDead(IBaseEventPayload payload)
    {
        Debug.Log("MonsterController Messaged");
        MonsterPayload monsterPayload = payload as MonsterPayload;

        //아이템 드롭
        DropItem(monsterPayload.ItemDrop, monsterPayload.Monster);
        StartCoroutine(RespawnMonster(monsterPayload.Monster, monsterPayload.RespawnTime));
    }

    private IEnumerator RespawnMonster(Transform transform, float time)
    {
        Debug.Log("Monster Dead");
        yield return new WaitForSeconds(monsterDisableWait);
        Debug.Log("Monster Disable");
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(time - monsterDisableWait);
        Debug.Log("Respawn");
        transform.localScale = originalScale;

        transform.gameObject.GetComponent<AbstractMonster>().ResetMonster();
    }

    private void DropItem(List<int> table, Transform monster)
    {
        List<(int, int)> dropItem = new();

        //Set Drop Item
        for (int i = 0; i < table.Count; i++)
        {
            int draw = Random.Range(0, 100);
            MonsterItemDropData itemData = DataManager.Instance.GetIndexData<MonsterItemDropData, MonsterItemDropDataParsingInfo>(table[i]);
            int j = 0;
            for (; j < itemData.maximumDrop; j++)
            {
                if (draw < itemData.noDropChance + itemData.addDropChance * j)
                {
                    dropItem.Add((j, itemData.dropItemIndex));
                    break;
                }
            }
            Debug.Log("Stone Quant : " + j);
        }

        //Drop Item
        for (int i = 0; i < dropItem.Count; i++)
        {
            if (dropItem[i].Item2<=4100) //Stone
            {
                StoneEventPayload payload = new()
                {
                    Type = StoneEventType.MineStone,
                    StoneSpawnPos = monster.position,
                    StoneForce = GetRandVector(),
                    StoneIdx = dropItem[i].Item2,
                };
                for (int j = 0; j < dropItem[i].Item1; j++)
                {
                    ticketMachine.SendMessage(ChannelType.Stone, payload);
                }
            }
            else //other Items
            {
                for(int j=0; j < dropItem[i].Item1;j++)
                {
                    Instantiate()
                }
            }
        }
    }
    
    private Vector3 GetRandVector()
    {
        Vector3 vec = new(Random.Range(-0.1f, -0.1f), 0.1f, Random.Range(-0.1f, -0.1f));
        return vec.normalized;
    }
}
