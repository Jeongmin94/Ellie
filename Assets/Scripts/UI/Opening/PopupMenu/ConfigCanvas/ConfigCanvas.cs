using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Data.UI.Opening;
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

        [Header("Config List")]
        [SerializeField]
        private ConfigType[] configTypes;

        [Header("UI Transform")]
        [SerializeField]
        private UITransformData buttonPanelTransform;

        [SerializeField] private UITransformData configListTransform;

        [Header("Button Typography")]
        [SerializeField]
        private TextTypographyData buttonTypographyData;

        [Header("List Typography")]
        [SerializeField]
        private TextTypographyData listTypographyData;

        private readonly TransformController buttonPanelController = new TransformController();
        private readonly TransformController configListController = new TransformController();

        private GameObject buttonPanel;
        private GameObject listPanel;

        private RectTransform buttonPanelRect;
        private RectTransform listPanelRect;

        private ConfigButtonPanel configToggleGroup;

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

            InitButtonToggleGroup();
        }

        private void InitButtonToggleGroup()
        {
            configToggleGroup = buttonPanel.GetOrAddComponent<ConfigButtonPanel>();
            configToggleGroup.InitConfigButtonPanel();
            configToggleGroup.InitConfigTypes(configTypes);
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