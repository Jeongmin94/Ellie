using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.GoogleSheet._8200GuideDialog
{
    [Serializable]
    public class GuideDialogData
    {
        public int index;
        public string message;
        public string speaker;
        public string imageName;

        // !TODO: 상황에 대한 처리?
    }

    [CreateAssetMenu(fileName = "GuideDialogData", menuName = "GameData List/GuideDialogData")]
    public class GuideDialogParsingInfo : DataParsingInfo
    {
        private const int InvalidIntValue = -1;

        public List<GuideDialogData> data = new List<GuideDialogData>();

        public override void Parse()
        {
            data.Clear();
            string[] lines = tsv.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                string[] entries = lines[i].Split('\t');
                GuideDialogData guideDialogData = new GuideDialogData();
                try
                {
                    // 0. Index
                    string strIndex = entries[0].Trim();
                    if (string.IsNullOrEmpty(strIndex))
                    {
                        guideDialogData.index = InvalidIntValue;
                    }
                    else
                    {
                        guideDialogData.index = int.Parse(strIndex);
                    }

                    // 1. Message
                    guideDialogData.message = entries[1].Trim();

                    // 2. Speaker
                    guideDialogData.speaker = entries[2].Trim();

                    // 3. 상황

                    // 4. ImageName
                    guideDialogData.imageName = entries[4].Trim();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{name} Parsing Error - line idx: {i}, line: {lines[i]}");
                    throw;
                }

                data.Add(guideDialogData);
            }
        }

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(GuideDialogData))
            {
                return data.Find(d => d.index == index) as T;
            }

            return default(T);
        }
    }
}