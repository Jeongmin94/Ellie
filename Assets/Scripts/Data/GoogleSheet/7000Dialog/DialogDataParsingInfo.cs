using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    public enum DialogSpeaker
    {
        NPC,
        Player,
        Narr,
    }
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
        private const int InvalidValue = -1;
        
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
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;
                string[] entries = lines[i].Split('\t');
                DialogData data = new DialogData();
                try
                {
                    //인덱스
                    string index = entries[0].Trim();
                    if (string.IsNullOrEmpty(index))
                    {
                        data.index = InvalidValue;
                    }
                    else
                    {
                        data.index = int.Parse(entries[0].Trim());
                    }

                    //대화 문자열
                    data.dialog = entries[1].Trim();
                    //화자(0은 npc, 1은 플레이어)
                    string speaker = entries[2].Trim();
                    if (string.IsNullOrEmpty(speaker))
                    {
                        data.speaker = 2;
                    }
                    else
                    {
                        data.speaker = int.Parse(entries[2].Trim());
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