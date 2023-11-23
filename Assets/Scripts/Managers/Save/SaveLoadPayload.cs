using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Inventory;
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
    }

    public class InventorySavePayload : IBaseEventPayload
    {
        // 데이터 저장에 필요한 타입들 정의
        public struct ItemSaveInfo
        {
            public const int InvalidIndex = -1;
            
            public GroupType groupType;
            public int itemIndex;
            public int itemCount;

            public int itemSlotIndex;
            public int equipmentSlotIndex;
        }

        public struct GoodsSaveInfo
        {
            public int goldAmount;
            public int stoneAmount;
        }

        private readonly List<ItemSaveInfo> saveInfos = new List<ItemSaveInfo>();
        public GoodsSaveInfo goodsSaveInfo;

        public void AddItemSaveInfo(ItemSaveInfo info) => saveInfos.Add(info);
        public List<ItemSaveInfo> GetItemSaveInfos() => saveInfos;
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