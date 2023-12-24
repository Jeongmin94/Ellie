using System.Collections.Generic;
using Item;
using UI.Inventory.CategoryPanel;
using UnityEngine;

namespace Data.UI.Inventory
{
    [CreateAssetMenu(fileName = "InventoryChannel", menuName = "UI/Inventory/InventoryChannel")]
    public class InventoryChannel : ScriptableObject
    {
        // on off

        // 아이템 추가
        // group type, baseItem

        // 아이템 장착
        // group type, baseItem

        // 아이템 삭제

        // 아이템 목록 관리

        private readonly IDictionary<GroupType, List<BaseItem>> items = new Dictionary<GroupType, List<BaseItem>>();
    }
}