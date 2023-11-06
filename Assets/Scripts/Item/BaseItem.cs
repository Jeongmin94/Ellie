using System;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseItem
    {
        public ItemData itemData;

        public int ItemCount
        {
            get => itemCount;
            set => itemCount = value;
        }

        public int ItemIndex => itemData.index;

        public Sprite ItemSprite { get; private set; }
        public string ItemName => itemData.name;

        private int itemCount = 0;

        public virtual void InitResources()
        {
            ItemSprite = ResourceManager.Instance.LoadSprite(itemData.imageName);
        }
    }
}