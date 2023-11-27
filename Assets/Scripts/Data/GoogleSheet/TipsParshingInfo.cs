using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TipData
{
    public int index;
    public string tip;
}


[CreateAssetMenu(fileName = "TipData", menuName = "GameData List/TipDats")]
public class TipsParshingInfo : DataParsingInfo
{
    public List<TipData> datas = new();

    public override T GetIndexData<T>(int index)
    {
        if(typeof(T)==typeof(TipData))
        {
            return datas.Find(m => m.index == index) as T;
        }
        return default(T);
    }

    public override void Parse()
    {
        datas.Clear();

        string[] lines = tsv.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;
            string[] entries = lines[i].Split('\t');

            TipData data = new TipData();

            try
            {
                //인덱스
                data.index = int.Parse(entries[0].Trim());
                //팁
                data.tip=entries[1].Trim();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(e);
                continue;
            }

            datas.Add(data);
        }
    }
}
