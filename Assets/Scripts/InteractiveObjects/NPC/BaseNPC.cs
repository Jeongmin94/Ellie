using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Channels.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class BaseNPC : MonoBehaviour, IInteractiveObject
    {
        //NPC 및 퀘스트 데이터
        protected NPCData npcData;
        public NPCData GetData() => npcData;
        //protected Dictionary<int, QuestData> questDataDict;
        protected QuestData curQuestData;

        public InteractiveType interactiveType = InteractiveType.Chatting;
        protected bool isInteracting;

        //NPC 오브젝트
        private Transform NPCObj;

        //플레이어 참조
        protected PlayerQuest player;

        //npc 로직 완료 후 비활성화 시 실행해줄 이벤트
        protected Action<string> OnDisableAction;

        [SerializeField] private int NPCIndex;
        [SerializeField] private float rotationSpeed;

        WaitForEndOfFrame wff = new WaitForEndOfFrame();

        private Coroutine _LookAtPlayer;

        private void Awake()
        {
            if (transform.childCount > 0)
                NPCObj = transform.GetChild(0);
        }

        private void Start()
        {
            
        }

        protected void Init()
        {
            StartCoroutine(InitCoroutine());
        }
        private IEnumerator InitCoroutine()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            npcData = DataManager.Instance.GetIndexData<NPCData, NPCDataParsingInfo>(NPCIndex);
            //questDataDict = new();
            //foreach(int dataIdx in npcData.questList)
            //{
            //    QuestData data = DataManager.Instance.GetIndexData<QuestData, QuestDataParsingInfo>(dataIdx);
            //    questDataDict.Add(dataIdx, data);
            //}
            //curQuestData = questDataDict[npcData.questList[0]];
        }
        public virtual void Interact(GameObject obj)
        {
            player = obj.GetComponent<PlayerQuest>();
            //LookAtPlayer();
        }

        protected void LookAtPlayer()
        {
            _LookAtPlayer = StartCoroutine(LookAtPlayerCoroutine());
        }
        private IEnumerator LookAtPlayerCoroutine()
        {
            if (player == null) yield break;
            while (true)
            {
                Vector3 direction = player.gameObject.transform.position - transform.position;
                direction.y = 0;

                Quaternion rotation = Quaternion.LookRotation(direction);
                NPCObj.rotation = Quaternion.Slerp(NPCObj.rotation, rotation, Time.deltaTime * rotationSpeed);

                float angleDifference = Quaternion.Angle(NPCObj.rotation, rotation);

                if (angleDifference <= 1.0f)
                {
                    //player.StartConversation();
                    yield break;
                }
                yield return wff;
            }
        }
        
        public void EndInteract()
        {
            if(_LookAtPlayer != null)
                StopCoroutine(_LookAtPlayer);
            //StartCoroutine(ResetRotation());
        }
        private IEnumerator ResetRotation()
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            while (Quaternion.Angle(NPCObj.rotation, targetRotation) > 1.0f)
            {
                NPCObj.rotation = Quaternion.Slerp(NPCObj.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }

            // 회전을 초기값으로 되돌린 후 코루틴 종료
            NPCObj.rotation = targetRotation;
        }

        public InteractiveType GetInteractiveType()
        {
            return interactiveType;
        }

        public void SubscribeOnDisableAction(Action<string> listener)
        {
            OnDisableAction -= listener;
            OnDisableAction += listener;
        }
    }
}