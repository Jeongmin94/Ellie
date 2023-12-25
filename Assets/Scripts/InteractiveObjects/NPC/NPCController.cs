using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using InteractingColliders;
using Managers.Save;
using UnityEngine;

namespace InteractiveObjects.NPC
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] private GameObject[] npcs;
        [SerializeField] private GameObject[] colliders;

        private Dictionary<NpcType, bool> NPCActiveDic = new();
        private readonly Dictionary<NpcType, GameObject> questColliderDic = new();

        private void Awake()
        {
            SaveLoadManager.Instance.SubscribeSaveEvent(SaveNPCData);
            SaveLoadManager.Instance.SubscribeLoadEvent(SaveLoadType.NPC, LoadNPCData);

            foreach (var npc in npcs)
            {
                npc.GetComponent<BaseNPC>().SubscribeOnDisableAction(OnNPCDisable);
            }

            foreach (var collider in colliders)
            {
                var type = collider.GetComponent<QuestCollider>().GetType();
                questColliderDic[type] = collider;
            }
        }

        private void OnNPCDisable(NpcType type)
        {
            NPCActiveDic[type] = false;
            questColliderDic[type].SetActive(false);

            StartCoroutine(SaveNPCStatusCoroutine());
        }

        private IEnumerator SaveNPCStatusCoroutine()
        {
            yield return SaveLoadManager.Instance.CheckIsSaveDone();
            SaveLoadManager.Instance.SaveData();
        }

        private void SaveNPCData()
        {
            var payload = new NPCSavePayload
            {
                NPCActiveDic = NPCActiveDic
            };
            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.NPC, payload);
        }

        private void LoadNPCData(IBaseEventPayload payload)
        {
            if (payload is not NPCSavePayload savePayload)
            {
                return;
            }

            NPCActiveDic = savePayload.NPCActiveDic;
            if (NPCActiveDic.Count > 0)
            {
                SetNPCActive();
            }
        }

        private void SetNPCActive()
        {
            foreach (var npcType in NPCActiveDic.Keys)
            {
                foreach (var npc in npcs)
                {
                    var type = npc.GetComponent<BaseNPC>().GetData().type;
                    if (type == npcType)
                    {
                        if (!NPCActiveDic[npcType])
                        {
                            npc.SetActive(false);
                            questColliderDic[type].SetActive(false);
                        }
                    }
                }
            }
        }
    }
}