using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public enum SaveLoadType
    {
        Test,
        Inventory,
        Quest,

        End,
    }

    public class TestSavePayload : IBaseEventPayload
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public List<Vector3> VectorList { get; set; } 
    }

    public class InventorySavePayload : IBaseEventPayload
    {
        // 데이터 저장에 필요한 타입들 정의
        // json에 2중 배열(크기 미지정)이나, Dictionary가 들어갈 수 있는지?
        // json에 프로퍼티로 저장 가능하고 읽어올 수 있는지??
        public List<List<int>> IndexItem { get; set; }
    }

    public class QuestSavePayload : IBaseEventPayload
    {
        // 데이터 저장에 필요한 타입들 정의
        public int QuestIndex { get; set; }
    }

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
                // 클래스를 json 변환
                string jsonData = SaveFile(i);

                if (jsonData == null)
                {
                    Debug.LogError($"{(SaveLoadType)i} 변환 실패");
                }

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
                        TestEventPayload payload = payloadTable[type] as TestEventPayload;
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
            // 데이터 로드해서 딕셔너리에 저장
            for (int i = 0; i < (int)SaveLoadType.End; i++)
            {
                IBaseEventPayload payload = LoadData(i);

                if (payload == null)
                {
                    Debug.LogError($"{(SaveLoadType)i} 변환 실패");
                }

                payloadTable[(SaveLoadType)i] = payload;
            }

            // 딕셔너리 돌면서 각 enum 타입에 맞는 payload들을 불러와서 Payload를 생성해서 인자로 연결된 함수에 전송
            foreach (var key in payloadTable.Keys)
            {
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