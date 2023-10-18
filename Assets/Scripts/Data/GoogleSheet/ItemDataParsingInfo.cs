using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public int index;
    public string name;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "GameData List/ItemData")]
public class ItemDataParsingInfo : DataParsingInfo
{
    public List<ItemData> items;
    
    public override void Parse()
    {
        
    }
}