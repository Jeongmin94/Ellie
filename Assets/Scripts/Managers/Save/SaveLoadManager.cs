using CodiceApp.EventTracking.Plastic;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Managers
{
    public enum SaveLoadType
    {
        Inventory,
        Quest,

        End,
    }

    public class InventorySavePayload : IBaseEventPayload
    {
        // 데이터 저장에 필요한 타입들 정의
    }

    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private string path;
        private string filename = "EllieTest";

        private Action saveAction;
        private Dictionary<SaveLoadType, Delegate> loadAction = new Dictionary<SaveLoadType, Delegate>();
        private Dictionary<SaveLoadType, IBaseEventPayload> payloadTable = new Dictionary<SaveLoadType, IBaseEventPayload>();

        public void SubscribeSaveFunc(Action listener)
        {
            saveAction -= listener;
            saveAction += listener;
        }

        public void SubscribeLoadFunc<T>(SaveLoadType eventName, Action<T> listener) where T : IBaseEventPayload
        {
            if (!loadAction.ContainsKey(eventName))
                loadAction[eventName] = null;
            loadAction[eventName] = Delegate.Combine(loadAction[eventName], listener);
        }

        public void UnsubscribeSaveFunc(Action listener)
        {
            saveAction -= listener;
        }

        public void UnsubscribeLoadFunc<T>(SaveLoadType eventName, Action<T> listener) where T : IBaseEventPayload
        {
            if (loadAction.ContainsKey(eventName))
                loadAction[eventName] = Delegate.Remove(loadAction[eventName], listener);
        }

        public void SaveData()
        {
            // 세이브 딕셔너리 저장
            saveAction?.Invoke();

            foreach(var kvp in payloadTable)
            {

            }
        }

        public void LoadData()
        {
            // 데이터 로드해서 딕셔너리에 저장
            for(int i = 0; i < (int)SaveLoadType.End; i++)
            {
                string data = File.ReadAllText(path + filename);
                IBaseEventPayload payload = JsonUtility.FromJson<IBaseEventPayload>(data);
                payloadTable[(SaveLoadType)i] = payload;
            }

            // 딕셔너리 돌면서 각 enum 타입에 맞는 payload들을 불러와서 Payload를 생성해서 인자로 연결된 함수에 전송
            foreach (var kvp in payloadTable.Keys)
            {
                ((Action<IBaseEventPayload>)loadAction[kvp])(payloadTable[kvp]);
            }
        }
    }
}