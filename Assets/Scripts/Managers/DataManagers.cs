using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManagers : Singleton<DataManagers>
{
    [SerializeField]
    private List<DataParsingInfo> dataList;

    private Dictionary<Type, DataParsingInfo> dataDictionary = new Dictionary<Type, DataParsingInfo>();

    private void Start()
    {
        foreach (var data in dataList)

        {
            dataDictionary[data.GetType()] = data;
        }
    }

    public T GetData<T>() where T : DataParsingInfo
    {
        if (dataDictionary.TryGetValue(typeof(T), out DataParsingInfo data))
        {
            return data as T;
        }
        else
        {
            Debug.LogError($"해당 데이터 타입 : {typeof(T)} 반환 불가");
            return null;
        }
    }
}
