using Assets.Scripts.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UI.Opening.PopupMenu.ConfigCanvas.ButtonPanel;
using UI.Opening.PopupMenu.ConfigCanvas.ListPanel.ConfigComponent;
using UnityEngine;

namespace Data.UI.Config.Save
{
    public static class ConfigDataHelper
    {
        public static void InitData<T>(BaseConfigOptionData<T>[] dataArray, ConfigType configType, UnityEngine.Transform transform)
        {
            foreach (var data in dataArray)
            {
                if (data.IsSameType(configType))
                {
                    data.ClearAction();
                    var component = UIManager.Instance.MakeSubItem<ConfigComponent>(transform, ConfigComponent.Path);
                    component.name += $"#{data.configName}";
                    component.SetConfigData(data.configName, data.readOnly, data.OnIndexChanged);
                    data.SubscribeValueChangeAction(component.OnOptionValueChanged);
                    data.InitData();
                }
            }
        }

        public static void SaveData<T>(BaseConfigOptionData<T>[] dataArray, ConfigType configType)
        {
            foreach (var data in dataArray)
            {
                if (data.readOnly)
                {
                    continue;
                }

                if (data.configType != configType)
                {
                    continue;
                }

                var jsonString = JsonConvert.SerializeObject(data);
                var jsonObj = JObject.Parse(jsonString);

                PlayerPrefs.SetInt(jsonObj["name"].ToString(), int.Parse(jsonObj["currentIdx"].ToString()));
            }

            PlayerPrefs.Save();
        }

        public static void LoadData<T>(BaseConfigOptionData<T>[] dataArray, ConfigType configType)
        {
            foreach (var data in dataArray)
            {
                if (data.readOnly)
                {
                    continue;
                }

                if (data.configType != configType)
                {
                    continue;
                }

                var jsonString = JsonConvert.SerializeObject(data);
                var jsonObj = JObject.Parse(jsonString);

                var idx = PlayerPrefs.GetInt(jsonObj["name"].ToString());
                data.currentIdx = idx;
            }
        }
    }
}