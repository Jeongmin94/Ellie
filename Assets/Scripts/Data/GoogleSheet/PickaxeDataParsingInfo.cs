using Assets.Scripts.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PickaxeData : ItemMetaData
{
    public int tier;
    public int durability;
    public int minSmithPower;
    public int maxSmithPower;
    public int upgradeCost;
    public float upgradeChance;
    public int uiIdx;
}

[CreateAssetMenu(fileName = "PickaxeData", menuName = "GameData List/PickaxeData")]

public class PickaxeDataParsingInfo : DataParsingInfo
{
    public List<PickaxeData> pickaxes;
    public override T GetIndexData<T>(int index)
    {
        if (typeof(T) == typeof(PickaxeData))
        {
            return pickaxes.Find(m => m.index == index) as T;
        }
        return default(T);
    }

    public override void Parse()
    {
        pickaxes.Clear();

        string[] lines = tsv.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            string[] entries = lines[i].Split('\t');

            PickaxeData data = new PickaxeData();
            try
            {
                data.groupType = Assets.Scripts.UI.Inventory.GroupType.Etc;
                //인덱스
                data.index = int.Parse(entries[0].Trim());
                //이름
                data.name = entries[1].Trim();
                //설명
                data.description = entries[2].Trim();
                //티어
                data.tier = int.Parse(entries[3].Trim());
                //내구도
                data.durability = int.Parse(entries[4].Trim());
                //최소 채광력
                data.minSmithPower = int.Parse(entries[5].Trim());
                //최대 채광력
                data.maxSmithPower = int.Parse(entries[6].Trim());
                //강화 비용
                data.upgradeCost = int.Parse(entries[7].Trim());
                //강화 확률
                data.upgradeChance = float.Parse(entries[8].Trim());
                //UI 이미지
                data.imageName = entries[9].Trim();
                //UI 인덱스
                data.uiIdx = int.Parse(entries[10].Trim());
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                Debug.LogError(e);
                continue;
            }
            pickaxes.Add(data);
        }
    }
}
