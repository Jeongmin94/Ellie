using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSaveLoad : MonoBehaviour
    {
        private PlayerController controller;
        private PlayerQuest quest;
        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            quest = GetComponent<PlayerQuest>();

            SaveLoadManager.Instance.SubscribeSaveEvent(SavePlayerData);
            SaveLoadManager.Instance.SubscribeLoadEvent(SaveLoadType.Player, LoadPlayerData);
        }

        public void SavePlayerData()
        {
            PlayerSavePayload payload = new PlayerSavePayload();
            payload.position = new Utils.SerializableVector3(transform.position);
            payload.questSaveInfo = quest.GetQuestDataSaveInfo();
            payload.pickaxeSaveInfo = controller.GetPickaxeDataSaveInfo();

            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.Player, payload);
        }

        public void LoadPlayerData(IBaseEventPayload payload)
        {
            if (payload is not PlayerSavePayload savePayload) return;
            transform.position = savePayload.position.ToVector3();
            quest.LoadQuestData(savePayload.questSaveInfo);
            controller.LoadPickaxeData(savePayload.pickaxeSaveInfo);
        }
    }
}