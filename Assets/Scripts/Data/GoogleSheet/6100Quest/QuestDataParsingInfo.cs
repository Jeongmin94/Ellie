using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    public enum QuestStatus
    {
        CantAccept,
        Unaccepted,
        Accepted,
        Done,
        End
    }

    [Serializable]
    public class QuestData
    {
        public int index;
        public string name;
        public string description;
        public int NPCIndex;
        public string questType;
        public string clearCondition;
        public string additionalCondition;
        public List<(int, int)> rewardList = new();
        public string playableText;
        public List<int> speechBubbleList = new();
        public List<int> cantAcceptDialogList = new();
        public List<int> unAcceptedDialogList = new();
        public List<int> AcceptedDialogList = new();
        public List<int> doneDialogList = new();
        public List<int> endDialogList = new();
        public List<int> additionalConditionDialogList = new();
        public int acceptanceTerm;
    }
    [CreateAssetMenu(fileName = "QuestData", menuName = "Quest/QuestData")]
    public class QuestDataParsingInfo : DataParsingInfo
    {
        private const int NONE = -1;
        public List<QuestData> questDatas = new();

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(QuestData))
            {
                return questDatas.Find(m => m.index == index) as T;
            }
            return default(T);
        }

        public override void Parse()
        {
            questDatas.Clear();
            string[] lines = tsv.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                string[] entries = lines[i].Split('\t');

                QuestData data = new QuestData();
                try
                {
                    //인덱스
                    data.index = int.Parse(entries[0].Trim());
                    //퀘스트이름
                    data.name = entries[1].Trim();
                    //퀘스트 설명
                    data.description = entries[2].Trim();
                    //담당 npc 인덱스
                    data.NPCIndex = int.Parse(entries[3].Trim());
                    //퀘스트 타입
                    data.questType = entries[4].Trim();
                    //클리어 조건
                    data.clearCondition = entries[5].Trim();
                    //부가 조건
                    data.additionalCondition = entries[6].Trim();
                    //보상 리스트
                    {
                        if(entries[7].Trim() != "-1")
                        {
                            string[] subEntries = entries[7].Split(';');
                            foreach (string subEntry in subEntries)
                            {
                                string[] temp = subEntry.Split(',');
                                int itemIdx = int.Parse(temp[0].Trim());
                                int count = int.Parse(temp[1].Trim());
                                data.rewardList.Add((itemIdx, count));
                            }
                        }
                        
                    }
                    //플레이블 텍스트
                    data.playableText = entries[8].Trim();
                    //말풍선 리스트
                    {
                        if (entries[9].Trim() != "-1")
                        {
                            string[] subEntries = entries[9].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.speechBubbleList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //수락할 수 없을 때 대화 리스트
                    {
                        if (entries[10].Trim() != "-1")
                        {
                            string[] subEntries = entries[10].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.cantAcceptDialogList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //수락할 수 있을 때 리스트
                    {
                        if (entries[11].Trim() != "-1")
                        {
                            string[] subEntries = entries[11].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.unAcceptedDialogList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //진행중 대화 리스트
                    {
                        if (entries[12].Trim() != "-1")
                        {
                            string[] subEntries = entries[12].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.AcceptedDialogList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //완료 대화 리스트
                    {
                        if (entries[13].Trim() != "-1")
                        {
                            string[] subEntries = entries[13].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.doneDialogList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //종료 후 대화 리스트
                    {
                        if (entries[14].Trim() != "-1")
                        {
                            string[] subEntries = entries[14].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.endDialogList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //부가 조건 대화 리스트
                    {
                        if (entries[15].Trim() != "-1")
                        {
                            string[] subEntries = entries[15].Split(";");
                            foreach (string subEntry in subEntries)
                            {
                                data.additionalConditionDialogList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    data.acceptanceTerm = int.Parse(entries[16].Trim());
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                    Debug.LogError(e);
                    continue;
                }
                questDatas.Add(data);
            }
        }
    }
}
