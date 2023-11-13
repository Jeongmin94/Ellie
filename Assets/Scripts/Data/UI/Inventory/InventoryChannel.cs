using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.UI.Inventory;
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