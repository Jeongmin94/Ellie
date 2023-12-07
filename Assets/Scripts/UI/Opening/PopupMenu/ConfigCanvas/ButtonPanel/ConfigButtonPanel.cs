using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Data.UI.Opening;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public enum ConfigType
    {
        Setting,
        Controls,
        Cheat,
    }

    public class ConfigButtonPanel : UIBase
    {
        private readonly List<ConfigToggleController> toggles = new List<ConfigToggleController>();

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