using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] private GameObject[] NPCs;

        private Dictionary<string, bool> NPCActiveDic = new Dictionary<string, bool>();
        private void Awake()
        {
            SaveLoadManager.Instance.SubscribeSaveEvent(SaveNPCData);
            SaveLoadManager.Instance.SubscribeLoadEvent(SaveLoadType.NPC, LoadNPCData);

            foreach (var npc in NPCs)
            {
                npc.GetComponent<BaseNPC>().SubscribeOnDisableAction(OnNPCDisable);
            }
        }

        private void OnNPCDisable(string name)
        {
            NPCActiveDic[name] = false;
            SaveLoadManager.Instance.SaveData();
        }

        private void SaveNPCData()
        {
            NPCSavePayload payload = new NPCSavePayload();
            payload.NPCActiveDic = NPCActiveDic;

            Debug.Log("Saving NPC Data");
            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.NPC, payload);
        }

        private void LoadNPCData(IBaseEventPayload payload)
        {
            Debug.Log("NPC Load");
            if (payload is not NPCSavePayload savePayload) return;

            NPCActiveDic = savePayload.NPCActiveDic;
            if(NPCActiveDic.Count > 0)
                SetNPCActive();
        }

        private void SetNPCActive()
        {
            foreach(var npcName in NPCActiveDic.Keys)
            {
                foreach(var npc in NPCs)
                {
                    if(npc.GetComponent<BaseNPC>().GetData().name == npcName)
                    {
                        if (!NPCActiveDic[npcName])
                            npc.SetActive(false);
                    }
                }
            }
        }
    }
}