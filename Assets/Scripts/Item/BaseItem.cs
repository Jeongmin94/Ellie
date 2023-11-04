using System;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseItem : MonoBehaviour
    {
        public ItemData itemData;
        public string ItemName => itemData.name;

        public int ItemCount
        {
            get => itemCount;
            set => itemCount = value;
        }

        private Sprite itemSprite;
        private string spriteName;
        private int itemCount = 0;

        protected virtual void InitResources()
        {
            itemSprite = ResourceManager.Instance.LoadSprite(itemData.spriteName);
        }
    }
}