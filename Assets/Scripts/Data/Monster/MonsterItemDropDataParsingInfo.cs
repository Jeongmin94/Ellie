using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class MonsterItemDropData
{
    public int index;
    public int dropItemIndex;
    public int noDropChance;
    public int addDropChance;
    public int maximumDrop;
}

[CreateAssetMenu(fileName = "MonsterItemDropData", menuName = "GameData List/Monsters/MonsterItemDropData", order = int.MaxValue)]
public class MonsterItemDropDataParsingInfo : DataParsingInfo
{
    public List<MonsterItemDropData> datas = new();

    public override T GetIndexData<T>(int index)
    {
        if(typeof(T)==typeof(MonsterItemDropData))
        {
            return datas.Find(m => m.index == index) as T;
        }
        return default(T);
    }

    public override void Parse()
    {
        datas.Clear();

        string[] lines = tsv.Split('\n');

        for(int i=0; i<lines.Length;i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;
            string[] entries = lines[i].Split('\t');

            MonsterItemDropData data = new();

            try
            {
                data.index = int.Parse(entries[0].Trim());
                data.dropItemIndex = int.Parse(entries[2].Trim());
                data.noDropChance = int.Parse(entries[3].Trim());
                data.addDropChance = int.Parse(entries[4].Trim());
                data.maximumDrop = int.Parse(entries[5].Trim());
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(ex);
                continue;
            }

            datas.Add(data);
        }
    }
}
