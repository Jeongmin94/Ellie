using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data.Quest
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
        public int idx;
        public string name;
        public string description;
        public QuestStatus status;
        public List<string>[] dialogList = new List<string>[(int)QuestStatus.End];

    }
    [CreateAssetMenu(fileName = "QuestDataList", menuName = "Quest/QuestDataList")]
    public class QuestDataList : ScriptableObject
    {
        public List<QuestData> questDataList = new();
    }
}
