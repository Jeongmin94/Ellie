using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

[Serializable]
public class ItemData
{
    public readonly GroupType groupType = GroupType.Consumption;

    public int index;
    public string name;
    public string description;
    public int appearanceStage;

    public string status;
    public int increasePoint;
    public int increasePercent;

    public string imageName;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "GameData List/ItemData")]
public class ItemDataParsingInfo : DataParsingInfo
{
    public List<ItemData> items = new List<ItemData>();

    public override T GetIndexData<T>(int index) where T : class
    {
        if (typeof(T) == typeof(OreData))
        {
            return items.Find(m => m.index == index) as T;
        }

        return default(T);
    }

    public override void Parse()
    {
        items.Clear();

        string[] lines = tsv.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            string[] entries = lines[i].Split('\t');

            ItemData data = new ItemData();

            try
            {
                //인덱스
                data.index = int.Parse(entries[0].Trim());
                //이름
                data.name = entries[1].Trim();
                // 설명
                data.description = entries[2].Trim();
                // 최초 등장 스테이지
                data.appearanceStage = int.Parse(entries[3].Trim());

                // 스테이터스 종류
                data.status = entries[4].Trim();
                // 증감 수치(int)
                data.increasePoint = int.Parse(entries[5].Trim());
                // 증감 수치(float)
                data.increasePercent = int.Parse(entries[6].Trim());

                // ui 이미지
                data.imageName = entries[7].Trim();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(e);
                continue;
            }

            items.Add(data);
        }
    }
}