using System;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
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
        }

        private enum Images
        {
            DialogImage,
        }

        private GameObject dialogPanel;
        private RectTransform dialogPanelRect;

        private Image dialogImage;

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

            dialogPanel = GetGameObject((int)GameObjects.DialogPanel);
            dialogPanelRect = dialogPanel.GetComponent<RectTransform>();

            dialogImage = GetImage((int)Images.DialogImage);
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(dialogPanelRect, AnchorPresets.MiddleCenter);
            dialogPanelRect.sizeDelta = DialogConst.DialogPanelRect.GetSize();
            dialogPanelRect.localPosition = DialogConst.DialogPanelRect.ToCanvasPos();
        }
    }
}