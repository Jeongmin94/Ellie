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
        [SerializeField] private StringOptionData[] stringOptionData;

        private enum GameObjects
        {
            Content,
        }

        public ConfigType ConfigMenuType { get; set; }

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

            foreach (var data in integerOptionData)
            {
                if (data.IsSameType(configType))
                {
                    data.ClearAction();

                    var component = UIManager.Instance.MakeSubItem<ConfigComponent>(content.transform, ConfigComponent.Path);
                    component.name += $"#{data.configName}";
                    component.SetConfigData(data.configName, data.readOnly, data.OnIndexChanged);
                    data.SubscribeValueChangeAction(component.OnOptionValueChanged);
                    data.OnIndexChanged(0);
                }
            }

            foreach (var data in vector2OptionData)
            {
                if (data.IsSameType(configType))
                {
                    data.ClearAction();

                    var component = UIManager.Instance.MakeSubItem<ConfigComponent>(content.transform, ConfigComponent.Path);
                    component.name += $"#{data.configName}";

                    component.SetConfigData(data.configName, data.readOnly, data.OnIndexChanged);
                    data.SubscribeValueChangeAction(component.OnOptionValueChanged);
                    data.OnIndexChanged(0);
                }
            }

            foreach (var data in stringOptionData)
            {
                if (data.IsSameType(configType))
                {
                    data.ClearAction();

                    var component = UIManager.Instance.MakeSubItem<ConfigComponent>(content.transform, ConfigComponent.Path);
                    component.name += $"#{data.configName}";

                    component.SetConfigData(data.configName, data.readOnly, data.OnIndexChanged);
                    data.SubscribeValueChangeAction(component.OnOptionValueChanged);
                    data.OnIndexChanged(0);
                }
            }
        }
    }
}