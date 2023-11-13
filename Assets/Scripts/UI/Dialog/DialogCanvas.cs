using System;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Data.UI.Dialog;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// !TODO: 대화창 UI 종류
// 1. 이름 + 대화
//      - 배경이 비쳐 보이는 이름과 대사창
//      - 넘기기 버튼으로 대사 넘기기 가능(스킵 없음)
//      - 플레이어의 조작권 회수 여부
// 2. 대화
//      - 대사창이 없는 상태로 대사를 출력
//      - 일정 시간이 지나면 자동으로 사라짐
//      - 조작권은 플레이어에게 있음

// !TODO: 대사 출력 방법
//      - 대사는 앞에서부터 순차적으로 출력
//      - 대사의 발화자가 바뀌거나, 대사창을 초과하는 경우에는 넘기기 버튼을 클릭할 때까지 대기
//      - 말풍선은 캐릭터의 머리 위로 투명도가 낮은 흰색 말풍선 사용
namespace Assets.Scripts.UI.Dialog
{
    public class DialogCanvas : UIPopup
    {
        public static readonly string Path = "Dialog/DialogCanvas";

        private enum GameObjects
        {
            DialogPanel,
            DialogNextPanel,
            DialogContextPanel,
        }

        private enum Images
        {
            DialogImage,
        }

        private enum Texts
        {
            DialogTitle,
            DialogNext,
        }

        [SerializeField] private DialogTypographyData dialogContextData;

        private GameObject dialogPanel;
        private RectTransform dialogPanelRect;

        private GameObject dialogNextPanel;
        private RectTransform dialogNextPanelRect;

        private GameObject dialogContextPanel;

        private Image dialogImage;

        private TextMeshProUGUI dialogTitle;
        private TextMeshProUGUI dialogNext;

        private DialogText dialogContextText;

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
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            dialogPanel = GetGameObject((int)GameObjects.DialogPanel);
            dialogPanelRect = dialogPanel.GetComponent<RectTransform>();

            dialogNextPanel = GetGameObject((int)GameObjects.DialogNextPanel);
            dialogNextPanelRect = dialogNextPanel.GetComponent<RectTransform>();

            dialogContextPanel = GetGameObject((int)GameObjects.DialogContextPanel);

            dialogImage = GetImage((int)Images.DialogImage);

            dialogTitle = GetText((int)Texts.DialogTitle);
            dialogNext = GetText((int)Texts.DialogNext);
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(dialogPanelRect, AnchorPresets.MiddleCenter);
            dialogPanelRect.sizeDelta = DialogConst.DialogPanelRect.GetSize();
            dialogPanelRect.localPosition = DialogConst.DialogPanelRect.ToCanvasPos();

            dialogNextPanelRect.SetParent(transform);
            {
                AnchorPresets.SetAnchorPreset(dialogNextPanelRect, AnchorPresets.MiddleCenter);
                dialogNextPanelRect.sizeDelta = DialogConst.DialogNextRect.GetSize();
                dialogNextPanelRect.localPosition = DialogConst.DialogNextRect.ToCanvasPos();
            }
            dialogNextPanelRect.SetParent(dialogPanelRect);

            dialogContextText = dialogContextPanel.gameObject.GetOrAddComponent<DialogText>();
            dialogContextText.InitDialogText();
            dialogContextText.InitTypography(dialogContextData);
        }

        private void Start()
        {
            // for test
            dialogContextText.Play("아무것도 먹지 못 한지 벌써 일주일이나 지났어", 0.01f);
        }
    }
}