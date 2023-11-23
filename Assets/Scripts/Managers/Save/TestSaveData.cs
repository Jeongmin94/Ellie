using Assets.Scripts.Utils;
using Channels.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TestSaveData : MonoBehaviour
    {
        public Transform player;

        private TestSavePayload testPayload;
        private SaveLoadType saveloadType = SaveLoadType.Test;

        private void Awake()
        {
            SaveLoadManager.Instance.SubscribeSaveEvent(Save);
            SaveLoadManager.Instance.SubscribeLoadEvent(saveloadType, Load);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Save");
                SaveLoadManager.Instance.SaveData();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Load");
                SaveLoadManager.Instance.LoadData();
            }
        }

        public void Save()
        {
            Debug.Log("세이브 호출은 되니?");
            // 원래 저장된 값을 불러와서 갱신시키기

            Vector3 playerPosition = player.position;
            testPayload = new TestSavePayload
            {
                Name = "test",
                Index = 2023,
                VectorList = new List<SerializableVector3>() { new SerializableVector3(new Vector3(3, 3, 3)), new SerializableVector3(new Vector3(2, 2, 2)) },
                Position = new SerializableVector3(playerPosition),
            };

            Debug.Log(testPayload.Name);
            Debug.Log(testPayload.Index);
            foreach (var vector in testPayload.VectorList)
            {
                Debug.Log(vector);
            }
            SaveLoadManager.Instance.AddPayloadTable(saveloadType, testPayload);
        }

        public void Load(IBaseEventPayload payload)
        {
            Debug.Log("로드 호출은 되니?");
            TestSavePayload testPayload = payload as TestSavePayload;

            Debug.Log(testPayload.Name);
            Debug.Log(testPayload.Index);
            foreach (var vector in this.testPayload.VectorList)
            {
                Debug.Log(vector);
            }

            player.position = testPayload.Position.ToVector3();
        }
    }
}