using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
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
            Debug.LogError("반환 실패");
            return null;
        }
    }

    public T GetIndexData<T, U>(int index) where U : DataParsingInfo where T : class
    {
        var data = GetData<U>();
        if(data != null)
        {
            return data.GetIndexData<T>(index);
        }
        else
        {
            return default(T);
        }
    }
}
