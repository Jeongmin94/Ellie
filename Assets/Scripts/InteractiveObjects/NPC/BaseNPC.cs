using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class BaseNPC : MonoBehaviour, IInteractiveObject
    {
        protected NPCData npcData;
        protected Dictionary<int, QuestData> questDataDict;
        protected QuestData curQuestData;
        protected bool isInteracting;
        private Transform NPCObj;
        private PlayerQuest player;

        [SerializeField] private int NPCIndex;
        [SerializeField] private float rotationSpeed;

        WaitForEndOfFrame wff = new WaitForEndOfFrame();

        //대사 출력
        
        // !TODO : GetComponent로 플레이어 컴포넌트 참조하는 로직을 채널 이용한 중개자 패턴으로 변경
        private void Awake()
        {
            if (transform.childCount > 0)
                NPCObj = transform.GetChild(0);
        }

        private void Start()
        {
            //npc데이터 초기화
            Init();
        }

        private void Init()
        {
            npcData = DataManager.Instance.GetIndexData<NPCData, NPCDataParsingInfo>(NPCIndex);
            questDataDict = new();
            foreach(int dataIdx in npcData.questList)
            {
                QuestData data = DataManager.Instance.GetIndexData<QuestData, QuestDataParsingInfo>(dataIdx);
                questDataDict.Add(dataIdx, data);
            }
            curQuestData = questDataDict[npcData.questList[0]];
        }
        public virtual void Interact(GameObject obj)
        {
            player = obj.GetComponent<PlayerQuest>();
            StartCoroutine(LookAtPlayerCoroutine());
        }

        private IEnumerator LookAtPlayerCoroutine()
        {
            while (true)
            {
                Vector3 direction = player.transform.position - NPCObj.transform.position;
                direction.y = 0;

                Quaternion rotation = Quaternion.LookRotation(direction);
                NPCObj.transform.rotation = Quaternion.Slerp(NPCObj.transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                float angleDifference = Quaternion.Angle(NPCObj.transform.rotation, rotation);

                if (angleDifference <= 1.0f)
                {
                    player.StartConversation();
                    yield break;
                }
                yield return wff;
            }
        }

        protected IEnumerator PlayDialog(int questIdx, QuestStatus status)
        {
            int curDialogIdx = 0;
            List<int> dialogList = new();
            switch(status)
            {
                case QuestStatus.CantAccept:
                    dialogList = questDataDict[questIdx].cantAcceptDialogList;
                    break;
                case QuestStatus.Unaccepted:
                    dialogList = questDataDict[questIdx].unAcceptedDialogList;
                    break;
                case QuestStatus.Accepted:
                    dialogList = questDataDict[questIdx].AcceptedDialogList;
                    break;
                case QuestStatus.Done:
                    dialogList = questDataDict[questIdx].doneDialogList;
                    break;
                case QuestStatus.End:
                    dialogList = questDataDict[questIdx].endDialogList;
                    break;
                default:
                    Debug.Log("없음");
                    break;
            }

            while(true)
            {
                if(Input.GetKeyDown(KeyCode.G))
                {
                    //다이얼로그 출력 로직
                    curDialogIdx++;
                }
                if(curDialogIdx == dialogList.Count)
                {
                    player.EndConversation();
                    yield return ResetRotation();
                }
                yield return wff;
            }
        }
        private IEnumerator ResetRotation()
        {
            while (Quaternion.Angle(NPCObj.transform.rotation, Quaternion.identity) > 1.0f)
            {
                NPCObj.transform.rotation = Quaternion.Slerp(NPCObj.transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
                yield return null;
            }

            // 회전을 초기값으로 되돌린 후 코루틴 종료
            transform.rotation = Quaternion.identity;
        }
    }
}