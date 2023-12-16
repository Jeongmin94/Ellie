using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
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
        private TalkingSkullEldestTriggerCollider triggerCollider;
        private enum EldestSkullQuest
        {
            quest6101 = 6101,
            quest6102,
        }
        // !TODO : NPC 로드하여 npc현재 active 상태를 판단하여 저장
        private void Start()
        {
            Init();
            triggerCollider = transform.GetChild(1).GetComponent<TalkingSkullEldestTriggerCollider>();
            triggerCollider.SubscribeFirstEncounterAction(OnFirstEncounterAction);
            triggerCollider.SubscribeSecondEncounterAction(OnSecondEncounterAction);
            triggerCollider.SubscribePlayerExitAction(OnPlayerExitAction);
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


        private void OnFirstEncounterAction(Collider other)
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

        private void OnSecondEncounterAction()
        {

            StartCoroutine(Quest6101Coroutine1());
        }

        private void OnPlayerExitAction()
        {
            StartCoroutine(Quest6101Coroutine3());
        }
        private void OnTriggerExit(Collider other)
        {
            //범위에서 나가면 다시 안으로 들어오게 하는 로직
            
        }


        private IEnumerator FirstEncounterCoroutine()
        {
            //첫 조우 시 코루틴
            //돌맹이 많이 주웠다는 대사 출력
            player.LockPlayerMovement();
            yield return StartCoroutine(player.DialogCoroutine(PLAYERFIRSTQUESTIDX, QuestStatus.Done));
            player.ActivateInteractiveUI();

            //보상 획득
            player.GetReward(PLAYERFIRSTQUESTIDX);
            player.UnlockPlayerMovement();
            //isEncountered를 true로
            //다음 퀘스트를 UnAccepted 상태로 변경
            player.SetQuestStatus((int)EldestSkullQuest.quest6101, QuestStatus.Unaccepted);
        }

        private IEnumerator Quest6101Coroutine1()
        {
            //6101의 unaccepted 상태의 대사들을 출력한 후, Accepted로 변경
            player.SetQuestStatus((int)EldestSkullQuest.quest6101, QuestStatus.Accepted);
            player.LockPlayerMovement();
            LookAtPlayer();
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Unaccepted));
            player.ActivateInteractiveUI();

            player.UnlockPlayerMovement();
        }
        private IEnumerator Quest6101Coroutine2()
        {
            //Accepted 상태의 대사들을 출력한 후, HitCount를 세고, 3번 이상 Hit했다면 그 다음 로직 실행
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Accepted));
            player.ActivateInteractiveUI();

            //말 다 했으면 고개 다시 돌림
            EndInteract();
            //플레이어 움직일 수 있게 풀어줘야됨
            player.EndConversation();
            //player.SetQuestStatus((int)EldestQuestList.quest6101, QuestStatus.Accepted);
            while (hitCount < 1)
            {
                yield return null;
            }
            //세 번 맞춘 후 0.5초 세고
            yield return new WaitForSeconds(0.5f);
            player.GetComponent<PlayerInteraction>().interactiveObject = this.gameObject;
            //플레이어 바라보고
            LookAtPlayer();
            player.StartConversation();
            //퀘스트 상태 바꿔주고
            player.SetQuestStatus((int)EldestSkullQuest.quest6101, QuestStatus.Done);
            //대사 출력하고
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Done));
            player.ActivateInteractiveUI();

            //보상주기
            player.GetReward((int)EldestSkullQuest.quest6101);
            //플레이어 움직일 수 있게 풀어줌
            player.DeactivateInteractiveUI();
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
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6101, QuestStatus.Accepted, true));
            player.ActivateInteractiveUI();

            player.UnlockPlayerMovement();
        }


        private IEnumerator Quest6102Coroutine1()
        {
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6102, QuestStatus.Unaccepted));
            player.ActivateInteractiveUI();

            EndInteract();
            player.EndConversation();
            player.SetQuestStatus((int)EldestSkullQuest.quest6102, QuestStatus.Done);
        }

        private IEnumerator Quest6102Coroutine2()
        {
            yield return StartCoroutine(player.DialogCoroutine((int)EldestSkullQuest.quest6102, QuestStatus.Done));
            EndInteract();
            player.EndConversation();
            player.GetReward((int)EldestSkullQuest.quest6102);
            player.SetQuestStatus((int)EldestSkullQuest.quest6102, QuestStatus.Done);
            player.SetQuestStatus(6103, QuestStatus.CantAccept);
            player.SetInteractiveObjToNull();
            //player.DeactivateInteractiveUI();
            
            OnDisableAction?.Invoke(npcData.type);
            this.gameObject.SetActive(false);
        }
        
        public void HitOnStone()
        {
            if (player == null || player.GetQuestStatus((int)EldestSkullQuest.quest6101) != QuestStatus.Accepted) return;
            if (hitCount < 1)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "NPC1", transform.position);
            }
            hitCount++;
            //player.
        }
    }
}