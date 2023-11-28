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
        public List<ConfigToggleText> Toggles => toggles;

        private readonly List<ConfigToggleText> toggles = new List<ConfigToggleText>();

        private RectTransform rect;
        private ToggleGroup toggleGroup;

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
                
                toggles.Add(toggle);
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
    }
}