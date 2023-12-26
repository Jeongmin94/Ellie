using Assets.Scripts.Data.GoogleSheet;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class TalkingSkullSecondNPC : BaseNPC
    {

        private enum SecondSkullQuest
        {
            Quest6103 = 6103,
            Quest6104,
            Quest6105,
        }
        private const int REQUIREDQUESTIDX = 6102;

        // !TODO : 함정과 이벤트 연결해서 피격판정 발생 시 체크
        // !TODO : 가방과 이벤트 연결해서 습득할 시 체크, 둘 다 체크된 경우 퀘스트 클리어

        public bool isTrapped = false;
        public bool hasBackpack = false;

        [SerializeField] SkullSecondTrap[] traps;
        [SerializeField] SkullSecondBackPack backPack;

        private void Start()
        {
            Init();
            foreach(var trap in traps)
                trap.SubscribeTrapHitAction(CheckTrap);
            backPack.SubscribeGetBackPackAction(CheckBackPack);
        }

        public override void Interact(GameObject obj)
        {
            base.Interact(obj);
            if (player.GetQuestStatus(REQUIREDQUESTIDX) == QuestStatus.Done)
            {
                LookAtPlayer();
                //player.StartConversation();


                StartCoroutine(Quest6103Coroutine1());
            }

            //6103 퀘스트가 Accepted 상태로 바뀌었으면
            if (player.GetQuestStatus((int)SecondSkullQuest.Quest6103) == QuestStatus.Accepted)
            {
                LookAtPlayer();
                player.StartConversation();


                StartCoroutine(Quest6103Coroutine2());
            }

            //6103 퀘스트가 Done으로 바뀌고 다시 말을 걸었을 경우
            if (player.GetQuestStatus((int)SecondSkullQuest.Quest6103) == QuestStatus.Done)
            {
                player.SetQuestStatus((int)SecondSkullQuest.Quest6104, QuestStatus.Unaccepted);
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6104Coroutine1());
            }

            //6014 퀘스트를 완료하지 못하고 말을 걸었을 경우
            if (player.GetQuestStatus((int)SecondSkullQuest.Quest6104) == QuestStatus.Accepted )
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6104Coroutine2());
            }

            //6014 퀘스트를 완료하고 말을 걸었을 경우
            if (player.GetQuestStatus((int)SecondSkullQuest.Quest6104) == QuestStatus.Done)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6104Coroutine3());
            }

            //6015퀘스트를 받을 수 있는 경우
            if (player.GetQuestStatus((int)SecondSkullQuest.Quest6105) == QuestStatus.Unaccepted)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6105Coroutine1());
            }

            //6105퀘스트 완료 후 말걸기

            if (player.GetQuestStatus((int)SecondSkullQuest.Quest6105) == QuestStatus.Done)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6105Coroutine2());

            }

        }

        private IEnumerator Quest6103Coroutine1()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            player.SetQuestStatus(REQUIREDQUESTIDX, QuestStatus.End);
            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6103, QuestStatus.Accepted));
            player.SetQuestStatus((int)SecondSkullQuest.Quest6103, QuestStatus.Accepted);
            player.ActivateInteractiveUI();

            //6013 퀘스트를 Accepted 상태로 변경
            EndInteract();
            //player.EndConversation();
        }

        private IEnumerator Quest6103Coroutine2()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6103, QuestStatus.Done));
            player.ActivateInteractiveUI();

            //6013 퀘스트를 Done 상태로 변경
            player.SetQuestStatus((int)SecondSkullQuest.Quest6103, QuestStatus.Done);
            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6104Coroutine1()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            //함정 피격 및 가방 가져오는 퀘스트
            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6104, QuestStatus.Unaccepted));
            player.ActivateInteractiveUI();

            //6014 퀘스트를 Accepted 상태로 변경
            player.SetQuestStatus((int)SecondSkullQuest.Quest6103, QuestStatus.End);
            player.SetQuestStatus((int)SecondSkullQuest.Quest6104, QuestStatus.Accepted);
            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6104Coroutine2()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            //6104 퀘스트를 완료하지 못한 경우
            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6104, QuestStatus.Accepted));
            player.ActivateInteractiveUI();

            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6104Coroutine3()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            //6104 퀘스틀 완료한경우
            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6104, QuestStatus.Done));
            player.ActivateInteractiveUI();

            player.GetReward((int)SecondSkullQuest.Quest6104);
            EndInteract();
            player.EndConversation();
            player.SetQuestStatus((int)SecondSkullQuest.Quest6104, QuestStatus.End);
            player.SetQuestStatus((int)SecondSkullQuest.Quest6105, QuestStatus.Unaccepted);
        }

        private IEnumerator Quest6105Coroutine1()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6105, QuestStatus.Unaccepted));
            player.ActivateInteractiveUI();

            player.SetQuestStatus((int)SecondSkullQuest.Quest6105, QuestStatus.Done);

            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6105Coroutine2()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }

            yield return StartCoroutine(player.DialogCoroutine((int)SecondSkullQuest.Quest6105, QuestStatus.Done));

            //player.SetQuestStatus((int)SecondSkullQuest.Quest6105, QuestStatus.End);
            player.SetQuestStatus(6106, QuestStatus.CantAccept);
            player.GetReward((int)SecondSkullQuest.Quest6105);

            EndInteract();
            player.EndConversation();

            player.SetInteractiveObjToNull();
            //player.DeactivateInteractiveUI();

            OnDisableAction?.Invoke(npcData.type);
            gameObject.SetActive(false);
        }


        private void CheckTrap()
        {
            isTrapped = true;
            if (hasBackpack)
                player.SetQuestStatus((int)SecondSkullQuest.Quest6104, QuestStatus.Done);
        }

        private void CheckBackPack()
        {
            hasBackpack = true;
            if (isTrapped)
                player.SetQuestStatus((int)SecondSkullQuest.Quest6104, QuestStatus.Done);

        }
    }
}