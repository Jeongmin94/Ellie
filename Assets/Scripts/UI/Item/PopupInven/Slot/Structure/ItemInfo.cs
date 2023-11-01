using UnityEngine;

namespace Assets.Scripts.UI.Item.PopupInven.Structure
{
    public struct ItemInfo
    {
        public Sprite ItemSprite { get; }

        public string ItemName { get; }

        public string ItemText { get; }

        public int ItemCount { get; }

        public ItemInfo(Sprite sprite, string itemName, string itemText, int itemCount)
        {
            ItemSprite = sprite;
            ItemName = itemName;
            ItemText = itemText;
            ItemCount = itemCount;
        }
    }
}