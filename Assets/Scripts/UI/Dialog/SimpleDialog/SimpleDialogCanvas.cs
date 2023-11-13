using Assets.Scripts.Data.UI.Dialog;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Dialog
{
    public class SimpleDialogCanvas : UIPopup
    {
        private enum GameObjects
        {
            SimpleDialogPanel,
        }

        [SerializeField] private DialogTypographyData dialogContextData;

        private GameObject simpleDialogPanel;
        private RectTransform simpleDialogPanelRect;

        private DialogText dialogText;

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

            simpleDialogPanel = GetGameObject((int)GameObjects.SimpleDialogPanel);
            simpleDialogPanelRect = simpleDialogPanel.GetComponent<RectTransform>();

            dialogText = simpleDialogPanel.GetOrAddComponent<DialogText>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(simpleDialogPanelRect, AnchorPresets.MiddleCenter);
            simpleDialogPanelRect.sizeDelta = DialogConst.SimpleDialogPanelRect.GetSize();
            simpleDialogPanelRect.localPosition = DialogConst.SimpleDialogPanelRect.ToCanvasPos();

            dialogText.InitDialogText();
            dialogText.InitTypography(dialogContextData);
        }
    }
}