using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterDropableItem", menuName = "GameData List/Monsters/MonsterDropableItemData", order = int.MaxValue)]
public class MonsterDropableItemData : ScriptableObject
{
    public List<DropItem> items;
}
