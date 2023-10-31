using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
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
            var uiTicket = new UITicket<IBaseEventPayload>();
            uiTicket.uiTicketAction -= OnUITicketAction;
            uiTicket.uiTicketAction += OnUITicketAction;
            ticketMachine.AddTicket(ChannelType.UI, uiTicket);
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

            // item 추가 테스트
            // 아이템 이미지, 아이템 이름, 아이템 텍스트
            var item = UIManager.Instance.MakeSubItem<SlotItem>(null, UIManager.UISlotItem);
            gridArea.AddItem(item);
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

        public void OnUITicketAction()
        {
            Debug.Log($"{gameObject.name} OnUITicketAction");
        }

        public void AddItem(ItemInfo itemInfo)
        {
            // item 추가 테스트
            // 아이템 이미지, 아이템 이름, 아이템 텍스트, 수량
            var item = UIManager.Instance.MakeSubItem<SlotItem>(null, UIManager.UISlotItem);

            gridArea.AddItem(item);
        }

        public void RemoveItem(ItemInfo itemInfo)
        {
        }

        private void InitDescPanel()
        {
        }
    }
}