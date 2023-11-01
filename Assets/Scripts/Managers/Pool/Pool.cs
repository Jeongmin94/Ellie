using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Managers
{
    public class Pool
    {
        public int MaxCount { get; set; } = 10;
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        private readonly Stack<Poolable> poolStack = new Stack<Poolable>();
        public Action<Pool> clearPoolAction;
        
        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        private Poolable Create()
        {
            GameObject go = Object.Instantiate(Original);
            go.name = Original.name;

            var poolable = go.GetOrAddComponent<Poolable>();
            PoolManager.Instance.poolTask.SetInfo(WithdrawScheduleInfo.Of(poolable, this));

            return poolable;
        }

        public int GetStackSize() => poolStack.Count;

        public void Push(Poolable poolable)
        {
            if (poolable)
            {
                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.isUsing = false;

                poolStack.Push(poolable);
                
                if (IsFull())
                    clearPoolAction?.Invoke(this);
            }
        }

        public Poolable Pop(Transform parent = null)
        {
            Poolable poolable;

            if (poolStack.Any())
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.transform.SetParent(parent);
            poolable.gameObject.SetActive(true);
            poolable.isUsing = true;

            return poolable;
        }

        public bool IsFull()
        {
            return poolStack.Count > MaxCount;
        }
    }
}