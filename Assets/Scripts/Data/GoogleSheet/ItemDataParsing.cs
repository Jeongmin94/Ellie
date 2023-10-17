using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int index;
    public string name;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "GameData List/ItemData")]
public class ItemDataParsing : DataParsing
{
    public List<ItemData> items;
    
    public override void Parse()
    {
        
    }
}