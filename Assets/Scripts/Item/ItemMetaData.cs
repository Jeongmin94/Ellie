using Assets.Scripts.UI.Inventory;
using System;

namespace Assets.Scripts.Item
{
    [Serializable]
    public class ItemMetaData
    {
        public GroupType groupType = GroupType.Stone;

        public int index;
        public string name;
        public string description;

        public string imageName;
    }
}
