using Assets.Scripts.Data.UI;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
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

        // !TODO: 다음 버튼(g) 눌렀을 때, 대화창 넘어가기 + 대화창 종료하기
    }
}