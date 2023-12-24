using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.GoogleSheet
{
    [Serializable]
    public class OreData
    {
        public int index;
        public string name;
        public int tier;
        public int HP;
        public int hardness;
        public List<(int, float)> miningEndDropItemList;
        public List<(int, float)> whileMiningDropItemList;
    }

    [CreateAssetMenu(fileName = "OreData", menuName = "GameData List/OreData")]
    public class OreDataParsingInfo : DataParsingInfo
    {
        public List<OreData> ores;

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(OreData))
            {
                return ores.Find(m => m.index == index) as T;
            }

            return default;
        }

        public override void Parse()
        {
            ores.Clear();

            var lines = tsv.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }

                var entries = lines[i].Split('\t');

                var data = new OreData();
                data.whileMiningDropItemList = new List<(int, float)>();
                data.miningEndDropItemList = new List<(int, float)>();

                try
                {
                    //인덱스
                    data.index = int.Parse(entries[0].Trim());
                    //이름
                    data.name = entries[1].Trim();
                    //티어
                    data.tier = int.Parse(entries[2].Trim());
                    data.HP = int.Parse(entries[3].Trim());
                    data.hardness = int.Parse(entries[4].Trim());
                    var whileMiningDropItem = entries[5].Trim().Split(',');
                    for (var j = 0; j < whileMiningDropItem.Length / 2; j++)
                    {
                        var dropTableIndex = int.Parse(whileMiningDropItem[2 * j].Trim());
                        var chance = float.Parse(whileMiningDropItem[2 * j + 1].Trim());
                        data.whileMiningDropItemList.Add((dropTableIndex, chance));
                    }

                    var miningEndDropItem = entries[6].Trim().Split(',');
                    for (var j = 0; j < miningEndDropItem.Length / 2; j++)
                    {
                        var dropTableIndex = int.Parse(miningEndDropItem[2 * j].Trim());
                        var chance = float.Parse(miningEndDropItem[2 * j + 1].Trim());
                        data.miningEndDropItemList.Add((dropTableIndex, chance));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                    Debug.LogError(e);
                    continue;
                }

                ores.Add(data);
            }
        }
    }
}