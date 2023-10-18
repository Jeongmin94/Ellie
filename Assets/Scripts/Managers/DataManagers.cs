using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagers : Singleton<DataManagers>
{
    Dictionary<int, DataParsingInfo> datas = new Dictionary<int, DataParsingInfo>();

    public DataParsingInfo GetData(int id)
    {
        if (datas.ContainsKey(id))
        {
            return datas[id];
        }
        else
        {
            return null;
        }
    }
}
