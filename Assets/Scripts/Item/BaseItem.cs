using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseItem : MonoBehaviour
    {
        public ItemData itemData;

        private Sprite sprite;
        
        // Sprite 등의 리소스 초기화
        protected virtual void InitResources()
        {
        } 
    }
}