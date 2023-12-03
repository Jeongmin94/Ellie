using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public enum SaveLoadType
    {
        Inventory,
        Test,
        Player,
        NPC,
        Puzzle,

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

        public readonly List<ItemSaveInfo> saveInfos = new List<ItemSaveInfo>();
        public GoodsSaveInfo goodsSaveInfo;

        public void AddItemSaveInfo(ItemSaveInfo info) => saveInfos.Add(info);
        public List<ItemSaveInfo> GetItemSaveInfos() => saveInfos;
    }

    public class PlayerSavePayload : IBaseEventPayload
    {
        //퀘스트 정보
        public struct QuestDataSaveInfo
        {
            public Dictionary<int, QuestStatus> questStatusDic;
        
            public QuestData curQuestData;
        }
        //곡괭이 정보

        public struct PickaxeDataSaveInfo
        {
            public bool isPickaxeAvailable;
            public int pickaxeTier;
        }
        //플레이어 위치
        public SerializableVector3 position { get; set; }

        public QuestDataSaveInfo questSaveInfo;
        public PickaxeDataSaveInfo pickaxeSaveInfo;
    }

    public class PuzzleSavePayload : IBaseEventPayload
    {

    }

    public class NPCSavePayload : IBaseEventPayload
    {

    }
    public class MapSavePayload : IBaseEventPayload
    {
        public SerializableVector3 Position { get; set; }
    }
}