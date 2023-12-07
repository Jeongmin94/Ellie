using System;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Data.UI.Config;
using Data.UI.Config.Save;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigMenuList : UIBase
    {
        public static readonly string Path = "Opening/ConfigMenuList";

        private static readonly string DataPath = "UI/ConfigData";

        [SerializeField] private BaseConfigOptionData<int>[] integerOptionData;
        [SerializeField] private BaseConfigOptionData<Vector2>[] vector2OptionData;
        [SerializeField] private BaseConfigOptionData<string>[] stringOptionData;

        private enum GameObjects
        {
            Content,
        }

        private ConfigType ConfigMenuType { get; set; }

        private GameObject content;

        private RectTransform rect;

        public void InitMenuList()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            content = GetGameObject((int)GameObjects.Content);

            rect = gameObject.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.StretchAll);
            rect.sizeDelta = Vector2.one;
            rect.localPosition = Vector3.one;
        }

        public void InitConfigComponents(ConfigType configType)
        {
            ConfigMenuType = configType;

            ConfigSaveData.InitData(integerOptionData, ConfigMenuType, content.transform);
            ConfigSaveData.InitData(stringOptionData, ConfigMenuType, content.transform);
            ConfigSaveData.InitData(vector2OptionData, ConfigMenuType, content.transform);
        }
    }
}