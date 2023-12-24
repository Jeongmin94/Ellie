using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Data.UI.Opening;
using UI.Framework;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine.UI;

namespace UI.Opening.PopupMenu.ConfigCanvas.ButtonPanel
{
    public enum ConfigType
    {
        Setting,
        Controls,
        Cheat
    }

    public class ConfigButtonPanel : UIBase
    {
        private readonly List<ConfigToggleController> toggles = new();
        private Action<PopupPayload> buttonPanelAction;

        private ToggleGroup toggleGroup;

        private void OnDestroy()
        {
            buttonPanelAction = null;
        }

        public void Subscribe(Action<PopupPayload> listener)
        {
            buttonPanelAction -= listener;
            buttonPanelAction += listener;
        }

        public void InitConfigButtonPanel()
        {
            Init();
        }

        public void InitConfigTypes(ConfigType[] types, TextTypographyData typographyData)
        {
            foreach (var type in types)
            {
                var toggle = UIManager.Instance.MakeSubItem<ConfigToggleText>(transform, ConfigToggleText.Path);
                toggle.InitConfigToggleText(toggleGroup);

                typographyData.title = ConfigCanvas.ConfigToName(type);

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