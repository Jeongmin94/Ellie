using Assets.Scripts.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TestSaveData : SerializedMonoBehaviour
    {
        public Transform player;

        [ShowInInspector] private TestSavePayload testPayload;
        private SerializableVector3 positionPayload;
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
                SaveData();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadData();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CheckFile();
            }
        }


        [Button("세이브", ButtonSizes.Large)]
        public void SaveData()
        {
            Debug.Log("Save");
            SaveLoadManager.Instance.SaveData();
        }

        [Button("로드", ButtonSizes.Large)]
        public void LoadData()
        {
            Debug.Log("Load");
            SaveLoadManager.Instance.LoadData();
        }

        [Button("세이브 파일 유무 체크", ButtonSizes.Large)]
        public void CheckFile()
        {
            Debug.Log(SaveLoadManager.Instance.IsSaveFilesExist());
        }

        public void Save()
        {
            // 원래 저장된 값을 불러와서 갱신시키기
            testPayload = new TestSavePayload
            {
                Name = "test",
                Index = 2023,
                VectorList = new List<SerializableVector3>() { new SerializableVector3(player.position), new SerializableVector3(new Vector3(2, 2, 2)) },
            };

            SaveLoadManager.Instance.AddPayloadTable(saveloadType, testPayload);
        }

        public void Load(IBaseEventPayload payload)
        {
            testPayload = payload as TestSavePayload;

            positionPayload = testPayload.VectorList[0];
            player.position = positionPayload.ToVector3();
        }
    }
}