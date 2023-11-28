using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Data.UI.Opening;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigMenuList : UIBase
    {
        public static readonly string Path = "Opening/ConfigMenuList";

        [SerializeField] private TextTypographyData typographyData;

        public ConfigType ConfigMenuType { get; set; }

        private enum GameObjects
        {
            ListPanel,
        }
        
        private GameObject listPanel;
        
        private RectTransform rect;
        private RectTransform listPanelRect;
        
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

            listPanel = GetGameObject((int)GameObjects.ListPanel);

            listPanelRect = listPanel.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(listPanelRect, AnchorPresets.StretchAll);
        }
    }
}