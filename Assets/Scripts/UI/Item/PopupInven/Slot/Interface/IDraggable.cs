using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public interface IDraggable
    {
        public BaseItem GetBaseItem();
        public void SetSlot(Transform parent);
        public bool IsOrigin();
    }
}