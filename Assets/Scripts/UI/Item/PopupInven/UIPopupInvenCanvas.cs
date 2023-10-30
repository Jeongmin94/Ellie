using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using UnityEngine;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public class UIPopupInvenCanvas : UIPopup
    {
        private enum GameObjects
        {
            DescPanel,
            SlotPanel,
            SlotAreaPosition
        }

        private GameObject slotPanelObject;
        private GameObject slotAreaPosition;
        private GameObject descPanelObject;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));

            slotPanelObject = GetGameObject((int)GameObjects.SlotPanel);
            slotAreaPosition = GetGameObject((int)GameObjects.SlotAreaPosition);
            descPanelObject = GetGameObject((int)GameObjects.DescPanel);

            InitSlotPanel();
        }

        private void InitSlotPanel()
        {
            var slotPanel = UIManager.Instance.MakeSubItem<SlotArea>(slotAreaPosition.transform, UIManager.UISlotArea);

            slotPanel.SetAnchorPreset(AnchorPresets.StretchAll);
            slotPanel.Reset();

            slotPanel.padding = 5;
            slotPanel.spacing = 5;
            slotPanel.row = 3;
            slotPanel.col = 8;

            slotPanel.InitSlotPanel();
        }

        private void InitDescPanel()
        {

        }
    }
}