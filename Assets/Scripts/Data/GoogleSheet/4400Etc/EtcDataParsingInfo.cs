using Assets.Scripts.Item;
using Assets.Scripts.UI.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet._4400Etc
{
    [Serializable]
    public class EtcData : ItemMetaData
    { }

    [CreateAssetMenu(fileName = "EtcData", menuName = "GameData List/EtcData")]

    public class EtcDataParsingInfo : DataParsingInfo
    {
        public List<EtcData> etcDatas = new List<EtcData>();

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(EtcData))
            {
                return etcDatas.Find(m => m.index == index) as T;
            }
            return default(T);
        }

        public override void Parse()
        {
            etcDatas.Clear();

            string[] lines = tsv.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                string[] entries = lines[i].Split('\t');

                EtcData data = new EtcData();

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
