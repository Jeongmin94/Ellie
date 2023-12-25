using UnityEngine;

namespace Managers.Pool
{
    public class Poolable : MonoBehaviour
    {
        public bool isUsing;

        public virtual void PoolableDestroy()
        {
            Destroy(gameObject);
        }
    }
}