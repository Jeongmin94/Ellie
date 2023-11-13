using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class DialogData
    {
        public int index;
        public string dialog;
        public int speaker;
    }
    [CreateAssetMenu(fileName = "DialogData", menuName = "GameData List/DialogData")]

    public class DialogDataParsingInfo : DataParsingInfo
    {
        public List<DialogData> datas = new();
        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(DialogData))
            {
                return datas.Find(m => m.index == index) as T;
            }

            return default(T);
        }

        public override void Parse()
        {
            datas.Clear();
            string[] lines = tsv.Split('\n');
            for(int i = 0; i<lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;
                string[] entries = lines[i].Split('\t');
                DialogData data = new DialogData();
                try
                {
                    //인덱스
                    data.index = int.Parse(entries[0].Trim()); 
                    //대화 문자열
                    data.dialog = entries[1].Trim();
                    //화자(0은 npc, 1은 플레이어)
                    data.speaker = int.Parse(entries[2].Trim());
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
