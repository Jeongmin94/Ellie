using System;
using System.Collections;
using Assets.Scripts.Managers;
using Channels.UI;
using Data.GoogleSheet._6000NPC;
using Data.GoogleSheet._6100Quest;
using Outline;
using Player;
using UnityEngine;

namespace InteractiveObjects.NPC
{
    public enum NpcType
    {
        TalkingSkullEldest,
        TalkingSkullSecond,
        TalkingSkullYoungest
    }

    public class BaseNPC : InteractiveObject
    {
        public InteractiveType interactiveType = InteractiveType.Chatting;

        [SerializeField] private int NPCIndex;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Renderer renderer;

        private Coroutine _LookAtPlayer;

        //protected Dictionary<int, QuestData> questDataDict;
        protected QuestData curQuestData;

        protected bool isInteracting;

        //NPC 및 퀘스트 데이터
        protected NPCData npcData;

        //NPC 오브젝트
        private Transform NPCObj;

        //npc 로직 완료 후 비활성화 시 실행해줄 이벤트
        protected Action<NpcType> OnDisableAction;

        //플레이어 참조
        protected PlayerQuest player;

        private readonly WaitForEndOfFrame wff = new();

        private void Awake()
        {
            if (transform.childCount > 0)
            {
                NPCObj = transform.GetChild(0);
            }
        }

        public NPCData GetData()
        {
            return npcData;
        }

        protected void Init()
        {
            StartCoroutine(InitCoroutine());
        }

        private IEnumerator InitCoroutine()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            npcData = DataManager.Instance.GetIndexData<NPCData, NPCDataParsingInfo>(NPCIndex);
        }

        public override void Interact(GameObject obj)
        {
            player = obj.GetComponent<PlayerQuest>();
        }

        protected void LookAtPlayer()
        {
            _LookAtPlayer = StartCoroutine(LookAtPlayerCoroutine());
        }

        private IEnumerator LookAtPlayerCoroutine()
        {
            if (player == null)
            {
                yield break;
            }

            while (true)
            {
                var direction = player.gameObject.transform.position - transform.position;
                direction.y = 0;

                var rotation = Quaternion.LookRotation(direction);
                NPCObj.rotation = Quaternion.Slerp(NPCObj.rotation, rotation, Time.deltaTime * rotationSpeed);

                var angleDifference = Quaternion.Angle(NPCObj.rotation, rotation);

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
            if (_LookAtPlayer != null)
            {
                StopCoroutine(_LookAtPlayer);
            }
            //StartCoroutine(ResetRotation());
        }

        private IEnumerator ResetRotation()
        {
            var targetRotation = Quaternion.Euler(0, 0, 0);
            while (Quaternion.Angle(NPCObj.rotation, targetRotation) > 1.0f)
            {
                NPCObj.rotation = Quaternion.Slerp(NPCObj.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }

            // 회전을 초기값으로 되돌린 후 코루틴 종료
            NPCObj.rotation = targetRotation;
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

        public void SubscribeOnDisableAction(Action<NpcType> listener)
        {
            OnDisableAction -= listener;
            OnDisableAction += listener;
        }
    }
}