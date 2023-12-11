using System;
using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigCanvas : UIPopup
    {
        public static readonly string Path = "ConfigPopupCanvas";

        private static readonly string SoundCancel = "click3";

        private enum GameObjects
        {
            ConfigButtonPanel,
            ConfigListPanel,
        }

        private enum Images
        {
            Background
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

        private readonly TransformController buttonPanelController = new TransformController();
        private readonly TransformController configListController = new TransformController();

        private GameObject buttonPanel;
        private GameObject listPanel;

        private RectTransform buttonPanelRect;
        private RectTransform listPanelRect;

        public Image Background => background;
        private Image background;

        public Action<PopupPayload> configCanvasAction;

        // 환경설정 UI 관리
        private ConfigButtonPanel configToggleGroup;
        private readonly List<ConfigMenuList> menuList = new List<ConfigMenuList>();

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            InputManager.Instance.Subscribe(InputType.Escape, OnEscapeAction);

            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            buttonPanel = GetGameObject((int)GameObjects.ConfigButtonPanel);
            listPanel = GetGameObject((int)GameObjects.ConfigListPanel);

            buttonPanelRect = buttonPanel.GetComponent<RectTransform>();
            listPanelRect = listPanel.GetComponent<RectTransform>();

            background = GetImage((int)Images.Background);
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
            InitMenuList();
        }

        private void InitButtonToggleGroup()
        {
            configToggleGroup = buttonPanel.GetOrAddComponent<ConfigButtonPanel>();
            configToggleGroup.InitConfigButtonPanel();
            configToggleGroup.InitConfigTypes(configTypes, buttonTypographyData);
            configToggleGroup.Subscribe(OnButtonPanelAction);
        }

        private void InitMenuList()
        {
            var configTypes = Enum.GetValues(typeof(ConfigType));
            for (int i = 0; i < configTypes.Length; i++)
            {
                var type = (ConfigType)configTypes.GetValue(i);
                var menu = UIManager.Instance.MakeSubItem<ConfigMenuList>(listPanelRect, ConfigMenuList.Path);
                menu.name += $"#{type}";
                menu.InitMenuList();
                menu.InitConfigComponents(type);
                menu.gameObject.SetActive(false);

                menuList.Add(menu);
            }
        }

        private void OnEscapeAction()
        {
            if (gameObject.activeSelf)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundCancel, Vector3.zero);
                PopupPayload payload = new PopupPayload();
                payload.buttonType = ButtonType.No;
                payload.popupType = PopupType.Config;
                configCanvasAction?.Invoke(payload);
            }
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            buttonPanelController.CheckQueue(buttonPanelRect);
            configListController.CheckQueue(listPanelRect);
#endif
        }

        #region ConfigToggleEvent

        private void OnButtonPanelAction(PopupPayload payload)
        {
            int idx = (int)payload.configType;
            menuList[idx].gameObject.SetActive(payload.isOn);
        }

        #endregion

        public static string ConfigToName(ConfigType type)
        {
            switch (type)
            {
                case ConfigType.Setting:
                    return "세팅";
                    break;
                case ConfigType.Controls:
                    return "컨트롤";
                    break;
                case ConfigType.Cheat:
                    return "치트";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}