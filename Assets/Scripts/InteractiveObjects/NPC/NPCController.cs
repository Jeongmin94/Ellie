using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using InteractingColliders;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] private GameObject[] npcs;
        [SerializeField] private GameObject[] colliders;
        
        private Dictionary<NpcType, bool> NPCActiveDic = new Dictionary<NpcType, bool>();
        private Dictionary<NpcType, GameObject> questColliderDic = new();
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
                NpcType type = collider.GetComponent<QuestCollider>().GetType();
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
            NPCSavePayload payload = new NPCSavePayload
            {
                NPCActiveDic = NPCActiveDic
            };
            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.NPC, payload);
        }

        private void LoadNPCData(IBaseEventPayload payload)
        {
            if (payload is not NPCSavePayload savePayload) return;
            NPCActiveDic = savePayload.NPCActiveDic;
            if(NPCActiveDic.Count > 0)
                SetNPCActive();
        }

        private void SetNPCActive()
        {
            foreach(var npcType in NPCActiveDic.Keys)
            {
                foreach(var npc in npcs)
                {
                    NpcType type = npc.GetComponent<BaseNPC>().GetData().type;
                    if(type == npcType)
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