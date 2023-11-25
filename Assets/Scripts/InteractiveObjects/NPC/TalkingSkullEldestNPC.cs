using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class TalkingSkullEldestNPC : BaseNPC
    {
        private const int PLAYERFIRSTQUESTIDX = 6100;
        private int hitCount = 0;

        private Coroutine Quest6101_2;
        private enum EldestSkullQuest
        {
            quest6101 = 6101,
            quest6102,
        }

        private void Start()
        {
            Init();
        }
        public override void Interact(GameObject obj)
        {
            //첫 조우해서 보상을 받은 이후부터 상호작용 가능
            if (player.GetQuestStatus(PLAYERFIRSTQUESTIDX) != QuestStatus.Done) return;
            //콜라이더 내에서 일정 거리 이하일 때만 상호작용 가능
            if (player == null || Vector3.Distance(player.transform.position, this.transform.position) > 3.0f) return;

            //고개 돌리기
            //첫 째가 상호작용할 때 로직 -> 대사 출력 후 6101퀘스트를 Accepted 상태로 변경
            if (player.GetQuestStatus((int)EldestSkullQuest.quest6101) == QuestStatus.Accepted)
            {
                LookAtPlayer();
                player.StartConversation();
                if (Quest6101_2 != null)
                    StopCoroutine(Quest6101_2);
                Quest6101_2 = StartCoroutine(Quest6101Coroutine2());
            }
            //6102 퀘스트라인
            if (player.GetQuestStatus((int)EldestSkullQuest.quest6102) == QuestStatus.Unaccepted)
            {
                LookAtPlayer();
                player.StartConversation();
                StartCoroutine(Quest6102Coroutine1());
            }

            if (player.GetQuestStatus((int)EldestSkullQuest.quest6102) == QuestStatus.Done)
            {
                LookAtPlayer();
                player.StartConversation();
                StartCoroutine(Quest6102Coroutine2());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //처음 enter 했을 시 6100퀘스트를 클리어하고 보상을 줌
            if (other.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<PlayerQuest>();
                //6100퀘스트가 수락상태라면 Done으로 변경
                if (player.GetQuestStatus(PLAYERFIRSTQUESTIDX) == QuestStatus.Accepted)
                {
                    player.SetQuestStatus(PLAYERFIRSTQUESTIDX, QuestStatus.Done);
                    //보상 주기
                    //player.PlayDialog(PLAYERFIRSTQUESTIDX, QuestStatus.Done);
                    //dialogCoroutine = StartCoroutine(player.DialogCoroutine(PLAYERFIRSTQUESTIDX, QuestStatus.Done));
                    StartCoroutine(FirstEncounterCoroutine());
                    //player.GetReward(PLAYERFIRSTQUESTIDX);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //범위에서 나가면 다시 안으로 들어오게 하는 로직
            if (!other.CompareTag("Player")) return;
            if (player.GetQuestStatus((int)EldestSkullQuest.quest6101) <= QuestStatus.Accepted)
                StartCoroutine(Quest6101Coroutine3());
        }


        private IEnumerator FirstEncounterCoroutine()
        {
            //첫 조우 시 코루틴
            //돌맹이 많이 주웠다는 대사 출력
            yield return StartCoroutine(player.DialogCoroutine(PLAYERFIRSTQUESTIDX, QuestStatus.Done, npcData.name));
            //보상 획득
            player.GetReward(PLAYERFIRSTQUESTIDX);
            //isEncountered를 true로
            //다음 퀘스트를 UnAccepted 상태로 변경
            player.SetQuestStatus((int)EldestSkullQuest.quest6101, QuestStatus.Unaccepted);
        }

        private IEnumerator Quest6101Coroutine1()
        {
            //6101의 unaccepted 상태의 대사들을 출력한 후, Accepted로 변경
            player.SetQuestStatus((int)EldestSkullQuest.quest6101, QuestStatus.Accepted);
            LookAtPlayer();
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Unaccepted, npcData.name));
        }
        private IEnumerator Quest6101Coroutine2()
        {
            //Accepted 상태의 대사들을 출력한 후, HitCount를 세고, 3번 이상 Hit했다면 그 다음 로직 실행
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Accepted, npcData.name));
            //말 다 했으면 고개 다시 돌림
            EndInteract();
            //플레이어 움직일 수 있게 풀어줘야됨
            player.EndConversation();
            //player.SetQuestStatus((int)EldestQuestList.quest6101, QuestStatus.Accepted);
            while (hitCount < 3)
            {
                yield return null;
            }
            //세 번 맞춘 후 0.5초 세고
            yield return new WaitForSeconds(0.5f);
            //플레이어 바라보고
            LookAtPlayer();
            player.StartConversation();
            //퀘스트 상태 바꿔주고
            player.SetQuestStatus((int)EldestSkullQuest.quest6101, QuestStatus.Done);
            //대사 출력하고
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Done, npcData.name));
            //보상주기
            player.GetReward((int)EldestSkullQuest.quest6101);
            //플레이어 움직일 수 있게 풀어줌
            player.EndConversation();
            EndInteract();

            //플레이어의 6102 퀘스트를 UnAccepted로 변경
            player.SetQuestStatus((int)EldestSkullQuest.quest6102, QuestStatus.Unaccepted);
        }

        private IEnumerator Quest6101Coroutine3()
        {
            //플레이어가 범위 밖으로 나가려고 할 때 실행되는 코루틴
            LookAtPlayer();
            player.GetBackToNPC(this.transform);
            player.LockPlayerMovement();
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Accepted, npcData.name, true));
            player.UnlockPlayerMovement();
        }


        private IEnumerator Quest6102Coroutine1()
        {
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6102, QuestStatus.Unaccepted, npcData.name));
            EndInteract();
            player.EndConversation();
            player.SetQuestStatus((int)EldestSkullQuest.quest6102, QuestStatus.Done);
        }

        private IEnumerator Quest6102Coroutine2()
        {
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6102, QuestStatus.Done, npcData.name));
            EndInteract();
            player.EndConversation();
            player.GetReward((int)EldestSkullQuest.quest6102);
            //player.SetQuestStatus((int)EldestSkullQuest.quest6102, QuestStatus.End);
            this.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (player != null &&
                player.GetQuestStatus((int)EldestSkullQuest.quest6101) == QuestStatus.Unaccepted &&
                Vector3.Distance(player.transform.position, transform.position) < 7.0f)
            {
                //첫 보상을 받았고 NPC와 얼추 더 가까워졌을 때 대사 출력
                StartCoroutine(Quest6101Coroutine1());
            }
        }

        public void HitOnStone()
        {
            if (player == null || player.GetQuestStatus((int)EldestSkullQuest.quest6101) != QuestStatus.Accepted) return;
            hitCount++;
        }
    }
}