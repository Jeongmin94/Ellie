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
        //NPC 및 퀘스트 데이터
        protected NPCData npcData;
        protected Dictionary<int, QuestData> questDataDict;
        protected QuestData curQuestData;

        protected bool isInteracting;

        //NPC 오브젝트
        private Transform NPCObj;

        //플레이어 참조
        protected PlayerQuest player;
        

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
            //npc데이터 초기화
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return DataManager.Instance.CheckIsParseDone();
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
            LookAtPlayer();
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
                Vector3 direction = player.gameObject.transform.position - NPCObj.transform.position;
                direction.y = 0;

                Quaternion rotation = Quaternion.LookRotation(direction);
                NPCObj.transform.rotation = Quaternion.Slerp(NPCObj.transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                float angleDifference = Quaternion.Angle(NPCObj.transform.rotation, rotation);

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
            StartCoroutine(ResetRotation());
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