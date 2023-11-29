using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigToggleText : OpeningText
    {
        public new static readonly string Path = "Opening/ConfigToggleText";
        public ConfigToggleController ToggleController => toggleController;
        private ConfigToggleController toggleController;

        public void InitConfigToggleText(ToggleGroup toggleGroup)
        {
            InitText();
            SetImageAlpha(0.0f);
            
            InitToggleController(toggleGroup);
        }

        private void InitToggleController(ToggleGroup toggleGroup)
        {
            toggleController = gameObject.GetOrAddComponent<ConfigToggleController>();
            toggleController.Init();
            toggleController.SetToggleGroup(toggleGroup);
        }
    }
}