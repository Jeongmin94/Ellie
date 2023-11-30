using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private string path;
        private string filename = "EllieTest";

        private Action saveAction;
        private Dictionary<SaveLoadType, Delegate> loadAction = new Dictionary<SaveLoadType, Delegate>();
        private Dictionary<SaveLoadType, IBaseEventPayload> payloadTable = new Dictionary<SaveLoadType, IBaseEventPayload>();

        public override void Awake()
        {
            base.Awake();

            path = Application.persistentDataPath + "/";
            Debug.Log($"등록 경로 :: {path}");
        }

        public void SubscribeSaveEvent(Action listener)
        {
            saveAction -= listener;
            saveAction += listener;
        }

        public void SubscribeLoadEvent(SaveLoadType eventName, Action<IBaseEventPayload> listener)
        {
            if (!loadAction.ContainsKey(eventName))
                loadAction[eventName] = null;
            loadAction[eventName] = Delegate.Combine(loadAction[eventName], listener);
        }

        public void UnsubscribeSaveEvnt(Action listener)
        {
            saveAction -= listener;
        }

        public void UnsubscribeLoadEvent<T>(SaveLoadType eventName, Action<T> listener) where T : IBaseEventPayload
        {
            if (loadAction.ContainsKey(eventName))
                loadAction[eventName] = Delegate.Remove(loadAction[eventName], listener);
        }

        public void AddPayloadTable(SaveLoadType type, IBaseEventPayload payload)
        {
            payloadTable[type] = payload;
        }

        public void SaveData()
        {
            // 세이브 딕셔너리 저장
            saveAction?.Invoke();

            for (int i = 0; i < (int)SaveLoadType.End; i++)
            {
                SaveLoadType type = (SaveLoadType)i;

                // payloadTable에서 해당 타입의 페이로드를 확인
                if (!payloadTable.ContainsKey(type) || payloadTable[type] == null)
                {
                    Debug.LogWarning($"{type} 세이브 실패");
                    continue;
                }

                // 클래스를 json 변환
                string jsonData = SaveFile(i);

                // 자동 생성 경로에 파일로 저장
                Debug.Log(path);
                File.WriteAllText(path + filename + i.ToString(), jsonData);
            }
        }

        private string SaveFile(int index)
        {
            SaveLoadType type = (SaveLoadType)index;

            switch (type)
            {
                case SaveLoadType.Test:
                    {
                        TestSavePayload payload = payloadTable[type] as TestSavePayload;
                        return JsonConvert.SerializeObject(payload);
                    }
                case SaveLoadType.Inventory:
                    {
                        InventorySavePayload payload = payloadTable[type] as InventorySavePayload;
                        return JsonConvert.SerializeObject(payload);
                    }
                case SaveLoadType.Quest:
                    {
                        QuestSavePayload payload = payloadTable[type] as QuestSavePayload;
                        return JsonConvert.SerializeObject(payload);
                    }
                default:
                    return null;
            }
        }

        public void LoadData()
        {
            for (int i = 0; i < (int)SaveLoadType.End; i++)
            {
                SaveLoadType type = (SaveLoadType)i;

                // 파일 경로 확인
                string filePath = path + filename + i.ToString();
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"{type} 로드 실패");
                    continue;
                }

                string data = File.ReadAllText(filePath);
                IBaseEventPayload payload = LoadData(i);

                if (payload == null)
                {
                    Debug.LogWarning($"{type} 로드 실패");
                    continue;
                }

                payloadTable[type] = payload;
            }

            // 딕셔너리 돌면서 각 enum 타입에 맞는 payload들을 불러와서 Payload를 생성해서 인자로 연결된 함수에 전송

            Debug.Log($"PayloadTable Size: {payloadTable.Values.Count}");
            foreach (var key in payloadTable.Keys)
            {
                Debug.Log($"Key: {key}");
                ((Action<IBaseEventPayload>)loadAction[key])(payloadTable[key]);
            }
        }

        private IBaseEventPayload LoadData(int index)
        {
            SaveLoadType type = (SaveLoadType)index;

            switch (type)
            {
                case SaveLoadType.Test:
                    {
                        string data = File.ReadAllText(path + filename + index.ToString());
                        return JsonConvert.DeserializeObject<TestSavePayload>(data);
                    }
                case SaveLoadType.Inventory:
                    {
                        string data = File.ReadAllText(path + filename + index.ToString());
                        return JsonConvert.DeserializeObject<InventorySavePayload>(data);
                    }
                case SaveLoadType.Quest:
                    {
                        string data = File.ReadAllText(path + filename + index.ToString());
                        return JsonConvert.DeserializeObject<QuestSavePayload>(data);
                    }
                default:
                    return null;
            }
        }
    }
}