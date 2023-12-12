using System;
using System.Collections.Generic;
using Assets.Scripts.InteractiveObjects.NPC;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class NPCData
    {
        public int index;
        public string name;
        public string description;
        public bool isInteractable;
        public bool isTakingControl;
        public List<int> questList = new();
        public NpcType type;
    }
    [CreateAssetMenu(fileName = "NPCData", menuName = "GameData List/NPCData")]

    public class NPCDataParsingInfo : DataParsingInfo
    {
        public List<NPCData> datas = new List<NPCData>();
        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(NPCData))
            {
                return datas.Find(m => m.index == index) as T;
            }

            return default(T);
        }

        public override void Parse()
        {
            datas.Clear();

            string[] lines = tsv.Split('\n');

            for(int i =  0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                string[] entries = lines[i].Split('\t');

                NPCData data = new NPCData();

                try
                {
                    //인덱스
                    data.index = int.Parse(entries[0].Trim());
                    //이름
                    data.name = entries[1].Trim();
                    // 설명
                    data.description = entries[2].Trim();
                    //상호작용 가능 여부
                    data.isInteractable = bool.Parse(entries[3].Trim());
                    //제어권 탈취 여부
                    data.isTakingControl = bool.Parse(entries[4].Trim());
                    //말풍선 대화 데이터 인덱스
                    {
                        if (entries[5].Trim() != "-1")
                        {
                            string[] subEntries = entries[5].Split(";");
                            foreach(string subEntry in subEntries)
                            {
                                data.questList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //enum타입
                    data.type = (NpcType)Enum.Parse(typeof(NpcType), entries[6].Trim());
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
}
