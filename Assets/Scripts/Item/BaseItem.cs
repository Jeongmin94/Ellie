using Assets.Scripts.ActionData;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseItem
    {
        public ItemData itemData;

        public readonly Data<int> itemCount = new Data<int>();

        public int ItemIndex => itemData.index;
        public Sprite ItemSprite { get; private set; }
        public string ItemName => itemData.name;

        public virtual void InitResources()
        {
            ItemSprite = ResourceManager.Instance.LoadSprite(itemData.imageName);
            itemCount.Value++;
        }
    }
}