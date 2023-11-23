using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public enum SaveLoadType
    {
        Test,
        Inventory,
        Quest,
        Map,

        End,
    }

    public class TestSavePayload : IBaseEventPayload
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public List<SerializableVector3> VectorList { get; set; }
        public SerializableVector3 Position { get; set; }
    }

    public class InventorySavePayload : IBaseEventPayload
    {
        // 데이터 저장에 필요한 타입들 정의

    }

    public class QuestSavePayload : IBaseEventPayload
    {
        // 데이터 저장에 필요한 타입들 정의
        public int QuestIndex { get; set; }
    }

    public class MapSavePayload : IBaseEventPayload
    {
        public SerializableVector3 Position { get; set; }
    }

}