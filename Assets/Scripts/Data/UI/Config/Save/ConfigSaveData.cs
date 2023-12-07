using Assets.Scripts.Managers;
using Assets.Scripts.UI.PopupMenu;
using UnityEngine;

namespace Data.UI.Config.Save
{
    [CreateAssetMenu(fileName = "ConfigSaveData", menuName = "UI/Config/ConfigSaveData", order = 0)]
    public class ConfigSaveData : ScriptableObject
    {
        public static void InitData<T>(BaseConfigOptionData<T>[] dataArray, ConfigType configType, Transform transform)
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

        public void SaveData<T>(BaseConfigOptionData<T>[] dataArray, ConfigType configType)
        {
            
        }
    }
}