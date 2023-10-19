using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    public override T GetIndexData<T>(int index) where T : class
    {
        throw new NotImplementedException();
    }

    public override void Parse()
    {

    }
}