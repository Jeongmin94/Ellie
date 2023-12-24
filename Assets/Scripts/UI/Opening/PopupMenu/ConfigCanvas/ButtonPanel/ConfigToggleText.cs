using UnityEngine.UI;
using Utils;

namespace UI.Opening.PopupMenu.ConfigCanvas.ButtonPanel
{
    public class ConfigToggleText : OpeningText
    {
        public new static readonly string Path = "Opening/ConfigToggleText";
        public ConfigToggleController ToggleController { get; private set; }

        public void InitConfigToggleText(ToggleGroup toggleGroup)
        {
            InitText();
            SetImageAlpha(0.0f);

            InitToggleController(toggleGroup);
        }

        private void InitToggleController(ToggleGroup toggleGroup)
        {
            ToggleController = gameObject.GetOrAddComponent<ConfigToggleController>();
            ToggleController.Init();
            ToggleController.SetToggleGroup(toggleGroup);
        }
    }
}