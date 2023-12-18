using Channels.Boss;
using Channels.Dialog;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class BossDialogData
    {
        public int index;
        // 상황 설명
        public string description;
        // 세이브 여부
        public bool isSaveDialog;
        // 다이얼로그 내용 리스트
        public List<BossDialog> dialogList;
    }
    [Serializable]
    public struct BossDialog
    {
        // 상황 타입
        public BossSituationType bossDialogType;
        // 다이얼로그 출력 타입
        public DialogCanvasType dialogCanvasType;
        // 발화자 (0 : 없음, 1 : 엘리, 2 : 첫째, 3 : 둘째, 4 : 셋째)
        public int speaker;
        // 다이얼로그 내용
        public string dialog;
        // 다이얼로그 출력 시간
        public float remainTime;
    }

    [CreateAssetMenu(fileName = "BossDialogData", menuName = "GameData List/BossDialogData")]
    public class BossDialogParsingInfo : DataParsingInfo
    {
        private const int InvalidValue = -1;

        public List<BossDialogData> datas = new();

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(BossDialogData))
            {
                return datas.Find(m => m.index == index) as T;
            }

            return default(T);
        }

        public override void Parse()
        {
            datas.Clear();
            BossDialogData currentData = null;

            foreach (string line in tsv.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;

                string[] entries = line.Split('\t');
                if (entries[0].Trim() != "-")
                {
                    // 새 인덱스에 대한 데이터 생성
                    if (currentData != null)
                    {
                        datas.Add(currentData);
                    }

                    currentData = new BossDialogData
                    {
                        index = int.Parse(entries[0].Trim()),
                        description = entries[1].Trim(),
                        isSaveDialog = bool.Parse(entries[2].Trim()),
                        dialogList = new List<BossDialog>()
                    };
                }

                if (currentData != null)
                {
                    // dialogList에 대화 추가
                    BossDialog dialog = new BossDialog
                    {
                        bossDialogType = entries[3].Trim() == "-" ?
                            BossSituationType.None : (BossSituationType)Enum.Parse(typeof(BossSituationType), entries[3].Trim()),
                        dialogCanvasType = (DialogCanvasType)Enum.Parse(typeof(DialogCanvasType), entries[4].Trim()),
                        speaker = int.Parse(entries[5].Trim()),
                        dialog = entries[6].Trim(),
                        remainTime = float.Parse(entries[7].Trim()),
                    };

                    currentData.dialogList.Add(dialog);
                }
            }

            // 마지막 데이터 추가
            if (currentData != null)
            {
                datas.Add(currentData);
            }
        }
    }
}