using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework.Popup;
using UnityEngine;

namespace UI.Dialog.GuideDialog
{
    // 특정 상황 최초 발생시 출력됨
    // 출력 후 n초 후에 사라짐
    // 동시에 여러 다이얼로그가 출력되면 이전 메시지 사라지고 출력됨
    // ex) 1번 , 2번 다이얼로그가 동시에 발생하면 1번 출력 -> 5초 표시 -> 1번 사라짐 -> 2번 출력
    public class GuideDialogCanvas : UIPopup
    {
        private struct GuideDialogInfo
        {
            private string speaker;
            private string message;
            private string image;

            private GuideDialogInfo(string speaker, string message, string image)
            {
                this.speaker = speaker;
                this.message = message;
                this.image = image;
            }

            public static GuideDialogInfo Of(string speaker, string message, string image)
            {
                return new GuideDialogInfo(speaker, message, image);
            }
        }

        // Speaker Image Base Path
        private static readonly string SpeakerImageBasePath = "UI/GuideDialog/SpeakerImage";

        private enum GameObjects
        {
            GuideDialogBackgroundPanel,
            GuideDialogImagePanel,
            GuideDialogSpeakerPanel,
            GuideDialogMessagePanel,
        }

        [SerializeField] private UITransformData[] transformData;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> panelRects = new List<RectTransform>();

        private readonly Queue<GuideDialogInfo> dialogInfoQueue = new Queue<GuideDialogInfo>();

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
        }

        private void Bind()
        {
        }

        private void InitObjects()
        {
        }
    }
}