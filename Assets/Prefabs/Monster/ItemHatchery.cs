using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemHatchery : MonoBehaviour
    {
        private const int poolSize = 10;
        private Pool itemPool;

        [SerializeField] private GameObject item;

        private void Awake()
        {
            InitPool();
        }
        private void InitPool()
        {
            itemPool = PoolManager.Instance.CreatePool(item, poolSize);
        }

        public void ProduceItem(Transform transform, int itemIndex)
        {
            Poolable obj = itemPool.Pop();
            obj.GetComponent<TestItemDrop>().data = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(itemIndex);
            //obj.GetComponent<TestItemDrop>().hatchery = this;
        }
    }
}