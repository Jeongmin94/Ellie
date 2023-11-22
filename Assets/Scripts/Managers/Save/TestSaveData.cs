using Channels.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TestSaveData : MonoBehaviour
    {
        private TestSavePayload test;
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
            // 원래 저장된 값을 불러와서 갱신시키기
            test = new TestSavePayload
            {
                Name = "test",
                Index = 2023,
                VectorList = new List<Vector3>() { new Vector3(3, 3, 3), new Vector3(2, 2, 2) }
            };

            SaveLoadManager.Instance.AddPayloadTable(saveloadType, test);
        }

        public void Load(IBaseEventPayload payload)
        {
            TestSavePayload testPayload = payload as TestSavePayload;

            Debug.Log(testPayload.Name);
            Debug.Log(testPayload.Index);
            foreach (var vector in testPayload.VectorList)
            {
                Debug.Log(vector);
            }
        }
    }
}