using Assets.Scripts.Centers;
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

            //퀘스트 디버그
            GetComponent<PlayerQuest>().DebugCurrentPlayerQuestDict();
            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.Player, payload);
        }

        public void LoadPlayerData(IBaseEventPayload payload)
        {
            if (payload is not PlayerSavePayload savePayload) return;

            Debug.Log("Player Load");

            transform.position = savePayload.position.ToVector3();
            quest.LoadQuestData(savePayload.questSaveInfo);
            controller.LoadPickaxeData(savePayload.pickaxeSaveInfo);

            StartCoroutine(SceneLoadCoroutine());
        }

        private IEnumerator SceneLoadCoroutine()
        {
            yield return SceneLoadManager.Instance.CheckIsLoadDone();

            controller.ChangeState(PlayerStateName.Start);
            InputManager.Instance.CanInput = true;
        }
    }
}