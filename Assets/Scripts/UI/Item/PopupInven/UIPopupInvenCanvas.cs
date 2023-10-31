using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Item.PopupInven.Structure;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
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
            // ItemMenuBackground
            ButtonPanel,
            GridPanel,
            SwitchingPanel,

            // DescMenu
        }

        #region ItemMenu

        [SerializeField] private int padding = 5;
        [SerializeField] private int spacing = 5;
        [SerializeField] private int row = 3;
        [SerializeField] private int col = 8;

        private const string PrefixButtonPath = "UI/Inven/Button/";
        private const string ButtonDefault = "TextBTN_Medium";
        private const string ButtonPressed = "TextBTN_Medium_Pressed";

        private GameObject buttonPanel;
        private GameObject gridPanel;
        private GameObject switchingPanel;

        private GridArea gridArea;
        private HorizontalGridArea switchingArea;

        private readonly List<Sprite> buttonSprites = new List<Sprite>();

        #endregion

        #region DescMenu

        #endregion

        private TicketMachine ticketMachine;

        private void Awake()
        {
            Init();

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotifyAction);
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));

            buttonPanel = GetGameObject((int)GameObjects.ButtonPanel);
            gridPanel = GetGameObject((int)GameObjects.GridPanel);
            switchingPanel = GetGameObject((int)GameObjects.SwitchingPanel);

            InitGridArea();
            InitButton();
            InitSwitchingArea();
        }

        private void InitGridArea()
        {
            gridArea = UIManager.Instance.MakeSubItem<GridArea>(gridPanel.transform, UIManager.UIGridArea);

            gridArea.SetGrid(padding, spacing, row, col);
            gridArea.InitGridArea();
            gridArea.InitSlot(row * col);
        }

        private void InitButton()
        {
            buttonSprites.Add(ResourceManager.Instance.LoadExternResource<Sprite>($"{PrefixButtonPath}{ButtonDefault}"));
            buttonSprites.Add(ResourceManager.Instance.LoadExternResource<Sprite>($"{PrefixButtonPath}{ButtonPressed}"));

            var button = UIManager.Instance.MakeSubItem<ItemMenuButton>(buttonPanel.transform, UIManager.UIItemMenuButton);

            button.SetText("Consumption");
            button.SetSprite(buttonSprites[(int)ButtonType.Pressed]);

            UIManager.Instance.MakeSubItem<ItemMenuButton>(buttonPanel.transform, UIManager.UIItemMenuButton);
            UIManager.Instance.MakeSubItem<ItemMenuButton>(buttonPanel.transform, UIManager.UIItemMenuButton);
        }

        private void InitSwitchingArea()
        {
            switchingArea = UIManager.Instance.MakeSubItem<HorizontalGridArea>(switchingPanel.transform, UIManager.UIHorizontalGridArea);

            switchingArea.SetHorizontalGrid(TextAnchor.MiddleCenter, false, false);
            switchingArea.InitGridArea();
            switchingArea.CreateSlot(3);
        }

        private void OnNotifyAction(IBaseEventPayload payload)
        {
            UIPayload uiPayload = payload as UIPayload;
            if (uiPayload.actionType == ActionType.AddSlotItem)
            {
                AddItem(new ItemInfo(uiPayload.sprite, uiPayload.name, uiPayload.text, uiPayload.count));
            }
            else if (uiPayload.actionType == ActionType.RemoveSlotItem)
            {
            }
        }

        private void AddItem(ItemInfo itemInfo)
        {
            // !TODO gridArea 분류별로 아이템 생성 요청
            gridArea.AddItem(itemInfo);
        }

        public void RemoveItem(ItemInfo itemInfo)
        {
        }

        private void InitDescPanel()
        {
        }
    }
}