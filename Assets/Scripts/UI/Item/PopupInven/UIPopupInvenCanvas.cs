using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using UnityEngine;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public enum SlotType
    {
        Consumption,
        Stone,
        Etc
    }

    public class UIPopupInvenCanvas : UIPopup
    {
        private enum GameObjects
        {
            // ItemMenu
            ButtonPanel,
            GridPanel,

            // DescMenu
        }

        #region ItemMenu

        [SerializeField] private int padding = 5;
        [SerializeField] private int spacing = 5;
        [SerializeField] private int row = 3;
        [SerializeField] private int col = 8;

        public int Padding => padding;
        public int Spacing => spacing;
        public int Row => row;
        public int Col => col;

        private GameObject buttonPanel;
        private GameObject gridPanel;

        private GridArea gridArea;

        #endregion

        #region DescMenu

        #endregion

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));

            buttonPanel = GetGameObject((int)GameObjects.ButtonPanel);
            gridPanel = GetGameObject((int)GameObjects.GridPanel);

            InitGridArea();
        }

        private void InitGridArea()
        {
            gridArea = UIManager.Instance.MakeSubItem<GridArea>(gridPanel.transform, UIManager.UIGridArea);

            gridArea.SetGrid(padding, spacing, row, col);
            gridArea.InitSlotPanel();
        }

        private void InitDescPanel()
        {
        }
    }
}