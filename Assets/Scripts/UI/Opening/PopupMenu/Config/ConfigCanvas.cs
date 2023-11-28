using System;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigCanvas : UIPopup
    {
        private enum GameObjects
        {
            ConfigButtonPanel,
            ConfigListPanel,
        }

        [SerializeField] private UITransformData buttonPanelTransform;
        [SerializeField] private UITransformData configListTransform;

        private readonly TransformController buttonPanelController = new TransformController();
        private readonly TransformController configListController = new TransformController();

        private GameObject buttonPanel;
        private GameObject listPanel;

        private RectTransform buttonPanelRect;
        private RectTransform listPanelRect;

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

            buttonPanel = GetGameObject((int)GameObjects.ConfigButtonPanel);
            listPanel = GetGameObject((int)GameObjects.ConfigListPanel);

            buttonPanelRect = buttonPanel.GetComponent<RectTransform>();
            listPanelRect = listPanel.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
#if UNITY_EDITOR
            buttonPanelTransform.actionRect.Subscribe(buttonPanelController.OnRectChange);
            buttonPanelTransform.actionScale.Subscribe(buttonPanelController.OnScaleChange);

            configListTransform.actionRect.Subscribe(configListController.OnRectChange);
            configListTransform.actionScale.Subscribe(configListController.OnScaleChange);
#endif
            AnchorPresets.SetAnchorPreset(buttonPanelRect, AnchorPresets.MiddleCenter);
            buttonPanelRect.sizeDelta = buttonPanelTransform.actionRect.Value.GetSize();
            buttonPanelRect.localPosition = buttonPanelTransform.actionRect.Value.ToCanvasPos();
            buttonPanelRect.localScale = buttonPanelTransform.actionScale.Value;

            AnchorPresets.SetAnchorPreset(listPanelRect, AnchorPresets.MiddleCenter);
            listPanelRect.sizeDelta = configListTransform.actionRect.Value.GetSize();
            listPanelRect.localPosition = configListTransform.actionRect.Value.ToCanvasPos();
            listPanelRect.localScale = configListTransform.actionScale.Value;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            buttonPanelController.CheckQueue(buttonPanelRect);
            configListController.CheckQueue(listPanelRect);
#endif
        }
    }
}