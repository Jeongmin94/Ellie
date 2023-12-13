using Assets.Scripts.Data.GoogleSheet;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class TalkingSkullYoungestNPC : BaseNPC
    {
        private enum YoungestSkullQuest
        {
            Quest6106 = 6106,
            Quest6107,
        }

        private const int REQUIREDQUESTIDX = 6105;
        private bool isPlayerGotPickaxe = false;
        
        [SerializeField] YoungestSkullPickaxe pickaxe;
        [SerializeField] Ore ore;

        private void Start()
        {
            Init();
            pickaxe.SubscribeGetPickaxeAction(OnPlayerAcquirePickaxeEvent);
            ore.SubscribeFirstMineAction(OnFirstMineEvent);
        }

        public override void Interact(GameObject obj)
        {
            base.Interact(obj);

            if (player.GetQuestStatus(REQUIREDQUESTIDX) == QuestStatus.Done)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6106Coroutine1());
            }

            if (player.GetQuestStatus((int)YoungestSkullQuest.Quest6106) == QuestStatus.Done)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6106Coroutine2());
            }

            //6107퀘스트 시작
            if (player.GetQuestStatus((int)YoungestSkullQuest.Quest6107) == QuestStatus.Unaccepted)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6107Coroutine1());
            }

            //6107퀘스트 조건 충족 전

            if (player.GetQuestStatus((int)YoungestSkullQuest.Quest6107) == QuestStatus.Accepted)
            {
                LookAtPlayer();
                player.StartConversation(); 


                StartCoroutine(Quest6107Coroutine2());
            }
            //6107퀘스트 조건 충족 후
            if (player.GetQuestStatus((int)YoungestSkullQuest.Quest6107) == QuestStatus.Done)
            {
                LookAtPlayer();
                player.StartConversation();

                StartCoroutine(Quest6107Coroutine3());
            }
        }

        private IEnumerator Quest6106Coroutine1()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            player.SetQuestStatus(REQUIREDQUESTIDX, QuestStatus.End);
            player.SetQuestStatus((int)YoungestSkullQuest.Quest6106, QuestStatus.Unaccepted);
            yield return StartCoroutine(player.DialogCoroutine((int)YoungestSkullQuest.Quest6106, QuestStatus.Unaccepted));
            player.ActivateInteractiveUI();

            player.SetQuestStatus((int)YoungestSkullQuest.Quest6106, QuestStatus.Done);
            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6106Coroutine2()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            yield return StartCoroutine(player.DialogCoroutine((int)YoungestSkullQuest.Quest6106, QuestStatus.Done));
            player.ActivateInteractiveUI();

            player.GetReward((int)YoungestSkullQuest.Quest6106);
            player.SetQuestStatus((int)YoungestSkullQuest.Quest6106, QuestStatus.End);
            player.SetQuestStatus((int)YoungestSkullQuest.Quest6107, QuestStatus.Unaccepted);
            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6107Coroutine1()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            yield return StartCoroutine(player.DialogCoroutine((int)YoungestSkullQuest.Quest6107, QuestStatus.Unaccepted));
            player.ActivateInteractiveUI();

            player.SetQuestStatus((int)YoungestSkullQuest.Quest6107, QuestStatus.Accepted);
            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6107Coroutine2()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            yield return StartCoroutine(player.DialogCoroutine((int)YoungestSkullQuest.Quest6107, QuestStatus.Accepted));
            player.ActivateInteractiveUI();

            EndInteract();
            player.EndConversation();
        }

        private IEnumerator Quest6107Coroutine3()
        {
            if (player == null)
            {
                Debug.Log("Player is Null");
                yield break;
            }
            yield return StartCoroutine(player.DialogCoroutine((int)YoungestSkullQuest.Quest6107, QuestStatus.Done));

            player.GetReward((int)YoungestSkullQuest.Quest6107);
            player.SetQuestStatus((int)YoungestSkullQuest.Quest6107, QuestStatus.End);
            EndInteract();
            player.EndConversation();
            player.SetInteractiveObjToNull();
            //player.DeactivateInteractiveUI();
            OnDisableAction?.Invoke(npcData.type);
            gameObject.SetActive(false);
        }

        private void OnPlayerAcquirePickaxeEvent()
        {
            isPlayerGotPickaxe = true;
        }

        private void OnFirstMineEvent()
        {
            player.SetQuestStatus((int)YoungestSkullQuest.Quest6107, QuestStatus.Done);
            ore.UnSubscribeFirstMineAction(OnFirstMineEvent);
        }
    }
}