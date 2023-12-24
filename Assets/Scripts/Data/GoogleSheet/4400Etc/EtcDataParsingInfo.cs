using System;
using System.Collections.Generic;
using Item;
using UI.Inventory.CategoryPanel;
using UnityEngine;

namespace Data.GoogleSheet._4400Etc
{
    [Serializable]
    public class EtcData : ItemMetaData
    {
    }

    [CreateAssetMenu(fileName = "EtcData", menuName = "GameData List/EtcData")]
    public class EtcDataParsingInfo : DataParsingInfo
    {
        public List<EtcData> etcDatas = new();

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(EtcData))
            {
                return etcDatas.Find(m => m.index == index) as T;
            }

            return default;
        }

        public override void Parse()
        {
            etcDatas.Clear();

            var lines = tsv.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }

                var entries = lines[i].Split('\t');

                var data = new EtcData();

                try
                {
                    data.groupType = GroupType.Etc;
                    //인덱스
                    data.index = int.Parse(entries[0].Trim());
                    //이름
                    data.name = entries[1].Trim();
                    // 설명
                    data.description = entries[2].Trim();

                    // ui 이미지
                    data.imageName = "UI/Item/Etc/" + entries[3].Trim();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                    Debug.LogError(e);
                    continue;
                }

                etcDatas.Add(data);
            }
        }
    }
}