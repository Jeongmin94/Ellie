using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigToggleText : OpeningText
    {
        public new static readonly string Path = "Opening/ConfigToggleText";
        
        private ConfigToggleController toggleController;

        // !TODO: 토글 텍스트 내부에서 토글 리스트 관리
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