using Item;
using UnityEngine;

namespace Player
{
    public class PlayerLooting : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var lootable = other.gameObject.GetComponent<ILootable>();
            if (lootable != null)
            {
                Accept(lootable);
            }
        }

        private void Accept(ILootable item)
        {
            item.Visit(this);
        }
    }
}