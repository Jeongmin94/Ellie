using Assets.Scripts.Data.Quest;
using System;
using System.Collections.Generic;
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
        public int speechBubbleIdx;
        public int questIdx;
        public List<int>[] dialogIndexList = new List<int>[(int)QuestStatus.End + 1];
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
                    data.speechBubbleIdx = int.Parse(entries[5].Trim());
                    //퀘스트 인덱스
                    data.questIdx = int.Parse(entries[6].Trim());

                    //퀘스트를 수락할 수 없을 때 대화 인덱스
                    string[] temp = entries[7].Split(",");
                    data.dialogIndexList[(int)QuestStatus.CantAccept] = new();
                    foreach (string entry in temp) 
                    {
                        if (entry.Equals("-1"))
                            break;
                        data.dialogIndexList[(int)QuestStatus.CantAccept].Add(int.Parse(entry.Trim()));
                    }
                    //수락할 수 있을 때 대화 인덱스
                    temp = entries[8].Split(",");
                    data.dialogIndexList[(int)QuestStatus.Unaccepted] = new();
                    foreach (string entry in temp)
                    {
                        if (entry.Equals("-1"))
                            break;
                        data.dialogIndexList[(int)QuestStatus.Unaccepted].Add(int.Parse(entry.Trim()));
                    }
                    //진행중 대화 인덱스
                    temp = entries[9].Split(",");
                    data.dialogIndexList[(int)QuestStatus.Accepted] = new();
                    foreach (string entry in temp)
                    {
                        if (entry.Equals("-1"))
                            break;
                        data.dialogIndexList[(int)QuestStatus.Accepted].Add(int.Parse(entry.Trim()));
                    }
                    //완료 시 대화 인덱스
                    temp = entries[10].Split(",");
                    data.dialogIndexList[(int)QuestStatus.Done] = new();
                    foreach (string entry in temp)
                    {
                        if (entry.Equals("-1"))
                            break;
                        data.dialogIndexList[(int)QuestStatus.Done].Add(int.Parse(entry.Trim()));
                    }
                    //퀘스트 종료 시 대화 인덱스
                    temp = entries[11].Split(",");
                    data.dialogIndexList[(int)QuestStatus.End] = new();
                    foreach (string entry in temp)
                    {
                        if (entry.Equals("-1"))
                            break;
                        data.dialogIndexList[(int)QuestStatus.End].Add(int.Parse(entry.Trim()));
                    }
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
