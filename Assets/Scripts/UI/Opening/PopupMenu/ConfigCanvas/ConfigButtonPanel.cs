using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Framework;
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
        public List<ConfigButtonToggleController> Toggle => toggles;

        private readonly List<ConfigButtonToggleController> toggles = new List<ConfigButtonToggleController>();

        private RectTransform rect;
        private ToggleGroup toggleGroup;

        public void InitConfigButtonPanel()
        {
            Init();
        }

        public void InitConfigTypes(ConfigType[] types)
        {
            // !TODO: ConfigType에 따라서 토글 버튼 + 메뉴 리스트 만들기
            foreach (var type in types)
            {
                switch (type)
                {
                    case ConfigType.Setting:
                        break;
                    case ConfigType.Controls:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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