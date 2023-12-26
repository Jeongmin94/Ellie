using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class Poolable : MonoBehaviour
    {
        public bool isUsing;

        public virtual void PoolableDestroy()
        {
            Destroy(this.gameObject);
        }
    }
}