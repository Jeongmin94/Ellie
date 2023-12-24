using System;
using UI.Inventory.CategoryPanel;

namespace Item
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