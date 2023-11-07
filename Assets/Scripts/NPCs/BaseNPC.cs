using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Data.Quest;
using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class BaseNPC : MonoBehaviour, IInteractiveObject
    {
        List<string>[] dialogList;
        private NPCData data;
        [SerializeField] private int NPCIndex;
        private bool isInteracting;
        private Transform NPCObj;
        private int dialogIdx;


        private void Awake()
        {
            if (transform.childCount > 0)
                NPCObj = transform.GetChild(0);
        }
        private void Start()
        {
            isInteracting = false;
            dialogIdx = 0;
            //StartCoroutine(InitNPC());
        }
        public void Interact(GameObject obj)
        {
            StartCoroutine(LookAtPlayerCoroutine(obj));
            obj.GetComponent<PlayerController>().StartConversation();
            isInteracting = true;
            //if (!data.isInteractable) return;

            //isInteracting = true;

            //if (data.isTakingControl)
            //{
            //    // !TODO : 플레이어를 InConversation 상태로 만드는 함수를 호출합니다
            //    obj.GetComponent<PlayerController>().StartConversation();
            //    //lerp로 플레이어 바라보게 하기
            //    StartCoroutine(LookAtPlayerCoroutine(obj));
            //}
            //else
            //{
            //    // !TODO : 제어권을 뺏지 않을 때의 대사를 출력합니다

            //}
            //obj.TryGetComponent(out PlayerQuest playerQuest);
            ////플레이어의 해당 퀘스트 상태를 가져옴
            //if (data.questIdx != -1)
            //{
            //    playerQuest.GetQuestStatus(data.questIdx);

            //}
        }

        private IEnumerator LookAtPlayerCoroutine(GameObject player)
        {
            Debug.Log("Look at player");
            while (true)
            {
                Vector3 direction = player.transform.position - NPCObj.transform.position;

                Quaternion rotation = Quaternion.LookRotation(direction);
                NPCObj.transform.rotation = Quaternion.Slerp(NPCObj.transform.rotation, rotation, Time.deltaTime * 10.0f);

                float angleDifference = Quaternion.Angle(NPCObj.transform.rotation, rotation);

                if (angleDifference <= 1.0f)
                    yield break;
                yield return null;
            }
        }
        public void EndInteract()
        {
            //NPCObj.transform.rotation = Quaternion.identity;
            StartCoroutine(ResetRotation());
            isInteracting = false;
            dialogIdx = 0;
        }

        private IEnumerator ResetRotation()
        {
            while (Quaternion.Angle(NPCObj.transform.rotation, Quaternion.identity) > 1.0f)
            {
                NPCObj.transform.rotation = Quaternion.Slerp(NPCObj.transform.rotation, Quaternion.identity, Time.deltaTime * 10.0f);
                yield return null;
            }

            // 회전을 초기값으로 되돌린 후 코루틴 종료
            transform.rotation = Quaternion.identity;
        }
        private IEnumerator InitNPC()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            data = DataManager.Instance.GetIndexData<NPCData, NPCDataParsingInfo>(NPCIndex);
        }

        private void StartConversation(QuestStatus status)
        {
            switch (status)
            {
                case QuestStatus.CantAccept:
                    //퀘스트 선행조건이 충족되지 않아 퀘스트를 수락할 수 없을 때
                    break;
                case QuestStatus.Unaccepted:
                    //퀘스트 선행조건을 만족하며, 수락하지 않았을 때
                    //이 대사에서 퀘스트를 줌
                    break;
                case QuestStatus.Accepted:
                    //퀘스트 진행중이며, 완료 조건을 충족하지 못했을 때
                    break;
                case QuestStatus.Done:
                    //퀘스트 진행중이며, 완료 조건을 충족했을 때
                    break;
                case QuestStatus.End:
                    //퀘스트를 끝내고 다시 말을 걸었을 때
                    break;
            }
        }
        // !TODO
        private void Update()
        {
            if (isInteracting)
            {

            }
        }
    }
}