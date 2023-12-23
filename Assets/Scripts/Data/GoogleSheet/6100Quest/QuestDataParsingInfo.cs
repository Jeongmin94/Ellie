using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        public string playableText;
        public List<int> speechBubbleList = new();
        public List<int> additionalConditionDialogList = new();
        public int acceptanceTerm;
        [ShowInInspector] public Dictionary<QuestStatus, List<int>> DialogListDic = new();
        public List<(int, int)> rewardList = new();
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

            return default;
        }

        public override void Parse()
        {
            questDatas.Clear();
            var lines = tsv.Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }

                var entries = lines[i].Split('\t');

                var data = new QuestData();
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
                        if (entries[7].Trim() != "-1")
                        {
                            var subEntries = entries[7].Split(';');
                            foreach (var subEntry in subEntries)
                            {
                                var temp = subEntry.Split(',');
                                var itemIdx = int.Parse(temp[0].Trim());
                                var count = int.Parse(temp[1].Trim());
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
                            var subEntries = entries[9].Split(";");
                            foreach (var subEntry in subEntries)
                            {
                                data.speechBubbleList.Add(int.Parse(subEntry.Trim()));
                            }
                        }
                    }
                    //수락할 수 없을 때 대화 리스트
                    {
                        if (entries[10].Trim() != "-1")
                        {
                            var subEntries = entries[10].Split(";");
                            List<int> cantAcceptDialogList = new();
                            foreach (var subEntry in subEntries)
                            {
                                cantAcceptDialogList.Add(int.Parse(subEntry.Trim()));
                            }

                            data.DialogListDic.Add(QuestStatus.CantAccept, cantAcceptDialogList);
                        }
                    }
                    //수락할 수 있을 때 리스트
                    {
                        if (entries[11].Trim() != "-1")
                        {
                            var subEntries = entries[11].Split(";");
                            List<int> unAcceptedDialogList = new();
                            foreach (var subEntry in subEntries)
                            {
                                unAcceptedDialogList.Add(int.Parse(subEntry.Trim()));
                            }

                            data.DialogListDic.Add(QuestStatus.Unaccepted, unAcceptedDialogList);
                        }
                    }
                    //진행중 대화 리스트
                    {
                        if (entries[12].Trim() != "-1")
                        {
                            var subEntries = entries[12].Split(";");
                            List<int> AcceptedDialogList = new();
                            foreach (var subEntry in subEntries)
                            {
                                AcceptedDialogList.Add(int.Parse(subEntry.Trim()));
                            }

                            data.DialogListDic.Add(QuestStatus.Accepted, AcceptedDialogList);
                        }
                    }
                    //완료 대화 리스트
                    {
                        if (entries[13].Trim() != "-1")
                        {
                            var subEntries = entries[13].Split(";");
                            List<int> doneDialogList = new();
                            foreach (var subEntry in subEntries)
                            {
                                doneDialogList.Add(int.Parse(subEntry.Trim()));
                            }

                            data.DialogListDic.Add(QuestStatus.Done, doneDialogList);
                        }
                    }
                    //종료 후 대화 리스트
                    {
                        if (entries[14].Trim() != "-1")
                        {
                            var subEntries = entries[14].Split(";");
                            List<int> endDialogList = new();
                            foreach (var subEntry in subEntries)
                            {
                                endDialogList.Add(int.Parse(subEntry.Trim()));
                            }

                            data.DialogListDic.Add(QuestStatus.End, endDialogList);
                        }
                    }
                    //부가 조건 대화 리스트
                    {
                        if (entries[15].Trim() != "-1")
                        {
                            var subEntries = entries[15].Split(";");
                            foreach (var subEntry in subEntries)
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