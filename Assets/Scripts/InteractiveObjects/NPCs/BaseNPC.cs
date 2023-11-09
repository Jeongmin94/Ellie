using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Data.Quest;
using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPCs
{
    public class BaseNPC : MonoBehaviour, IInteractiveObject
    {
        protected NPCData data;
        protected bool isInteracting;
        private Transform NPCObj;

        [SerializeField] private int NPCIndex;
        [SerializeField] private float rotationSpeed;

        // !TODO : GetComponent로 플레이어 컴포넌트 참조하는 로직을 채널 이용한 중개자 패턴으로 변경
        private void Awake()
        {
            if (transform.childCount > 0)
                NPCObj = transform.GetChild(0);
        }
        private void Start()
        {
            isInteracting = false;
            StartCoroutine(InitNPC());
        }
        public virtual void Interact(GameObject obj)
        {
            //if (isInteracting) return;
            StartCoroutine(LookAtPlayerCoroutine(obj));
            isInteracting = true;
        }

        private IEnumerator LookAtPlayerCoroutine(GameObject player)
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
                    player.GetComponent<PlayerController>().StartConversation();
                    yield break;
                }
                yield return null;
            }
        }
        public void EndInteract()
        {
            StartCoroutine(ResetRotation());
            isInteracting = false;
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
        private IEnumerator InitNPC()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            data = DataManager.Instance.GetIndexData<NPCData, NPCDataParsingInfo>(NPCIndex);
        }

        protected QuestStatus CheckQuestStatus(GameObject obj)
        {
            return obj.GetComponent<PlayerQuest>().GetQuestStatus(data.questIdx);
        }
    }
}