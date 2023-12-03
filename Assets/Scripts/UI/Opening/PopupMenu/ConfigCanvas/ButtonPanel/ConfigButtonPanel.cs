using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public enum ConfigType
    {
        Setting,
        Controls
    }

    public class ConfigButtonPanel : UIBase
    {
        public List<ConfigToggleController> Toggles => toggles;

        private readonly List<ConfigToggleController> toggles = new List<ConfigToggleController>();

        private RectTransform rect;
        private ToggleGroup toggleGroup;
        private Action<PopupPayload> buttonPanelAction;

        public void Subscribe(Action<PopupPayload> listener)
        {
            buttonPanelAction -= listener;
            buttonPanelAction += listener;
        }

        private void OnDestroy()
        {
            buttonPanelAction = null;
        }

        public void InitConfigButtonPanel()
        {
            Init();
        }

        private List<string> buttonNames = new List<string>() { "세팅", "컨트롤" };

        public void InitConfigTypes(ConfigType[] types, TextTypographyData typographyData)
        {
            // !TODO: ConfigType에 따라서 토글 버튼 + 메뉴 리스트 만들기
            foreach (var type in types)
            {
                var toggle = UIManager.Instance.MakeSubItem<ConfigToggleText>(transform, ConfigToggleText.Path);
                toggle.InitConfigToggleText(toggleGroup);

                int idx = (int)type;
                switch (type)
                {
                    case ConfigType.Setting:
                    {
                        typographyData.title = buttonNames[idx];
                    }
                        break;
                    case ConfigType.Controls:
                    {
                        typographyData.title = buttonNames[idx];
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                toggle.name += $"#{typographyData.title}";
                toggle.InitTypography(typographyData);
                toggle.ToggleController.ToggleConfigType = type;

                toggles.Add(toggle.ToggleController);
            }

            InitMenuList(toggles);
        }

        private void InitMenuList(List<ConfigToggleController> controllers)
        {
            foreach (var controller in controllers)
            {
                controller.Subscribe(OnToggleAction);
            }
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            rect = GetComponent<RectTransform>();
            toggleGroup = GetComponent<ToggleGroup>();
        }

        private void InitObjects()
        {
            toggleGroup.SetAllTogglesOff();
        }

        #region ConfigToggleEvent

        private void OnToggleAction(PopupPayload payload)
        {
            buttonPanelAction?.Invoke(payload);
        }

        #endregion
    }
}