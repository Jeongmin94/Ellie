using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PoolManager : Singleton<PoolManager>
    {
        public float cleanInterval = 0.1f;

        private readonly IDictionary<string, Pool> pools = new Dictionary<string, Pool>();
        public readonly PoolTask poolTask = new();

        private Transform root;

        public override void Awake()
        {
            base.Awake();
            root = Instance.transform;

            poolTask.schedulePushAction -= OnSchedulePushAction;
            poolTask.schedulePushAction += OnSchedulePushAction;
        }

        private void Update()
        {
            poolTask.Handle();
        }

        public Pool CreatePool(GameObject original, int count = 5)
        {
            var pool = new Pool();
            pool.Init(original, count);

            pool.Root.SetParent(root);
            pool.clearPoolAction -= OnClearPoolAction;
            pool.clearPoolAction += OnClearPoolAction;

            pools.TryAdd(original.name, pool);

            return pool;
        }

        public Pool GetPool(string poolName)
        {
            if (pools.TryGetValue(poolName, out var pool))
            {
                return pool;
            }

            return null;
        }

        public void Push(Poolable poolable)
        {
            var objectName = poolable.name;
            var info = poolTask.GetInfo(poolable);
            if (pools.TryGetValue(objectName, out var pool) && info != null)
            {
                OnSchedulePushAction(info);
            }
            else
            {
                Destroy(poolable.gameObject);
            }
        }

        // n초 후 자동 Push
        public void Push(Poolable poolable, float time)
        {
            if (time <= 0.0f)
            {
                Push(poolable);
                return;
            }

            var objectName = poolable.name;
            var info = poolTask.GetInfo(poolable);
            if (pools.TryGetValue(objectName, out var pool) && info != null)
            {
                var version = info.Reserve();
                Task.Run(async () =>
                {
                    var prevVersion = version;

                    await Task.Delay((int)time * 1000);

                    if (info.Validate(prevVersion))
                    {
                        poolTask.EnqueueInfo(info);
                    }
                });
            }
            else
            {
                Destroy(poolable.gameObject);
            }
        }

        public Poolable Pop(GameObject original, Transform parent = null)
        {
            if (!pools.ContainsKey(original.name))
            {
                CreatePool(original);
            }

            if (parent == null)
            {
                parent = root;
            }

            return pools[original.name].Pop(parent);
        }

        public GameObject GetOriginal(string prefabName)
        {
            if (pools.TryGetValue(prefabName, out var pool))
            {
                return pool.Original;
            }

            return null;
        }

        public void DestroyAllPools()
        {
            foreach (Transform child in root)
            {
                Destroy(child.gameObject);
            }

            pools.Clear();
        }

        private void OnClearPoolAction(Pool pool)
        {
            if (!pool.IsFull())
            {
                return;
            }

            StartCoroutine(ClearPool(pool));
        }

        private IEnumerator ClearPool(Pool pool)
        {
            var wfs = new WaitForSeconds(cleanInterval);
            while (pool.IsFull())
            {
                var top = pool.Pop();

                top.PoolableDestroy();
                yield return wfs;
            }
        }

        private void OnSchedulePushAction(WithdrawScheduleInfo info)
        {
            info.Cancel();
            info.pool.Push(info.poolable);
        }
    }
}