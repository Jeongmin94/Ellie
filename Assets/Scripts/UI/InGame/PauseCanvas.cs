using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class PauseCanvas : UIPopup
    {
        private enum GameObjects
        {
            ButtonPanel,
            EscapePanel,
        }

        private enum Images
        {
            EscapeImage,
        }

        [SerializeField] private UITransformData buttonPanelTransformData;
        [SerializeField] private UITransformData escapeTransformData;
        [SerializeField] private UITransformData escapeImageTransformData;

        [SerializeField] private TextTypographyData escapeTypographyData;

        [Header("메뉴 이름")] [SerializeField] private string[] menuTitles;

        private GameObject buttonPanel;
        private GameObject escapePanel;

        private RectTransform buttonPanelRect;
        private RectTransform escapePanelRect;
        private RectTransform escapeImageRect;

        private Image escapeImage;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            buttonPanel = GetGameObject((int)GameObjects.ButtonPanel);
            escapePanel = GetGameObject((int)GameObjects.EscapePanel);

            escapeImage = GetImage((int)Images.EscapeImage);

            buttonPanelRect = buttonPanel.GetComponent<RectTransform>();
            escapePanelRect = escapePanel.GetComponent<RectTransform>();
            escapeImageRect = escapeImage.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            // !TODO Odin Inspector 오류 수정 필요
            buttonPanelTransformData.actionRect.Value = new Rect(173, 540, 383, 111 * menuTitles.Length);
            buttonPanelTransformData.actionScale.Value = Vector2.one;

            escapeTransformData.actionRect.Value = new Rect(1691, 922, 130, 70);
            escapeTransformData.actionScale.Value = Vector2.one;

            escapeImageTransformData.actionRect.Value = new Rect(1632, 925, 71, 61);
            escapeImageTransformData.actionScale.Value = Vector2.one;
            //

            AnchorPresets.SetAnchorPreset(buttonPanelRect, AnchorPresets.MiddleCenter);
            buttonPanelRect.sizeDelta = buttonPanelTransformData.actionRect.Value.GetSize();
            buttonPanelRect.localPosition = buttonPanelTransformData.actionRect.Value.ToCanvasPos();
            buttonPanelRect.localScale = buttonPanelTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(escapePanelRect, AnchorPresets.MiddleCenter);
            escapePanelRect.sizeDelta = escapeTransformData.actionRect.Value.GetSize();
            escapePanelRect.localPosition = escapeTransformData.actionRect.Value.ToCanvasPos();
            escapePanelRect.localScale = escapeTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(escapeImageRect, AnchorPresets.MiddleCenter);
            escapeImageRect.sizeDelta = escapeImageTransformData.actionRect.Value.GetSize();
            escapeImageRect.localPosition = escapeImageTransformData.actionRect.Value.ToCanvasPos();
            escapeImageRect.localScale = escapeTransformData.actionScale.Value;
        }
    }
}