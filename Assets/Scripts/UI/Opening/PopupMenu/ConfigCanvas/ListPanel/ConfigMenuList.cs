using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Data.UI.Config;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigMenuList : UIBase
    {
        public static readonly string Path = "Opening/ConfigMenuList";

        [SerializeField] private IntegerOptionData[] integerOptionData;
        [SerializeField] private Vector2OptionData[] vector2OptionData;

        private enum GameObjects
        {
            Content,
        }

        public ConfigType ConfigMenuType { get; set; }

        private GameObject content;

        private RectTransform rect;
        private RectTransform contentRect;

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
            contentRect = content.GetComponent<RectTransform>();
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

            foreach (var data in integerOptionData)
            {
                if (data.IsSameType(configType))
                {
                    var component = UIManager.Instance.MakeSubItem<ConfigComponent>(rect, ConfigComponent.Path);
                }
            }

            foreach (var data in vector2OptionData)
            {
                if (data.IsSameType(configType))
                {
                }
            }
        }
    }
}