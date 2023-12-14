using System;
using System.Collections.Generic;
using Assets.Scripts.Centers;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.PopupMenu;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Opening
{
    public class OpeningCanvas : UIStatic
    {
        public static readonly string Path = "Opening/OpeningCanvas";

        private static readonly string SoundOpeningBGM = "BGM1";

        private enum GameObjects
        {
            TitlePanel,
            MenuPanel,
            OuterRim
        }

        private GameObject titlePanel;
        private GameObject menuPanel;
        private GameObject outerRim;

        private RectTransform titlePanelRect;
        private RectTransform menuPanelRect;
        private RectTransform outerRimRect;

        [SerializeField] private bool useText = true;
        [SerializeField] private UITransformData titleTransformData;
        [SerializeField] private UITransformData menuTransformData;
        [SerializeField] private TextTypographyData titleTypographyTypographyData;

        [Header("Menu Button Data")]
        [SerializeField]
        private TextTypographyData[] buttonsData;

        private readonly TransformController titleController = new TransformController();
        private readonly TransformController menuController = new TransformController();

        private OpeningText title;

        private readonly List<BlinkMenuButton> menuButtons = new List<BlinkMenuButton>();
        private readonly List<BasePopupCanvas> popupCanvasList = new List<BasePopupCanvas>();
        private ConfigCanvas configCanvas;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, SoundOpeningBGM);

            if (!useText)
            {
                title.gameObject.SetActive(false);
                titlePanel.GetComponent<Image>().SetNativeSize();
            }
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            titlePanel = GetGameObject((int)GameObjects.TitlePanel);
            menuPanel = GetGameObject((int)GameObjects.MenuPanel);
            outerRim = GetGameObject((int)GameObjects.OuterRim);

            titlePanelRect = titlePanel.GetComponent<RectTransform>();
            menuPanelRect = menuPanel.GetComponent<RectTransform>();
            outerRimRect = outerRim.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
#if UNITY_EDITOR
            titleTransformData.actionRect.Subscribe(titleController.OnRectChange);
            titleTransformData.actionScale.Subscribe(titleController.OnScaleChange);

            menuTransformData.actionRect.Subscribe(menuController.OnRectChange);
            menuTransformData.actionScale.Subscribe(menuController.OnScaleChange);
#endif

            AnchorPresets.SetAnchorPreset(titlePanelRect, AnchorPresets.MiddleCenter);
            titlePanelRect.sizeDelta = titleTransformData.actionRect.Value.GetSize();
            titlePanelRect.localPosition = titleTransformData.actionRect.Value.ToCanvasPos();
            titlePanelRect.localScale = titleTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(menuPanelRect, AnchorPresets.MiddleCenter);
            menuPanelRect.sizeDelta = menuTransformData.actionRect.Value.GetSize();
            menuPanelRect.localPosition = menuTransformData.actionRect.Value.ToCanvasPos();
            menuPanelRect.localScale = menuTransformData.actionScale.Value;

            InitTitle();
            InitMenuButtons();
            InitPopupCanvas();

            // 231130 �߰�
            CheckLoadFiles();
        }

        private void InitTitle()
        {
            title = UIManager.Instance.MakeSubItem<OpeningText>(titlePanelRect, OpeningText.Path);
            title.InitText();
            title.InitTypography(titleTypographyTypographyData);
        }

        private void InitMenuButtons()
        {
            var popupTypes = Enum.GetValues(typeof(PopupType));
            for (int i = 0; i < buttonsData.Length; i++)
            {
                var type = (PopupType)popupTypes.GetValue(i);
                var button = UIManager.Instance.MakeSubItem<BlinkMenuButton>(menuPanelRect, BlinkMenuButton.Path);
                button.name += $"#{buttonsData[i].title}";
                button.InitText();
                button.InitTypography(buttonsData[i]);
                button.Subscribe(OnBlinkButtonAction);
                button.PopupType = type;

                menuButtons.Add(button);
            }
        }

        private void InitPopupCanvas()
        {
            var popupTypes = Enum.GetValues(typeof(PopupType));
            for (int i = 0; i < buttonsData.Length; i++)
            {
                var type = (PopupType)popupTypes.GetValue(i);
                var popup = UIManager.Instance.MakePopup<BasePopupCanvas>(BasePopupCanvas.Path);
                popup.InitPopupCanvas(type);
                popup.Subscribe(OnPopupCanvasAction);
                popup.name = $"#{buttonsData[i].title}";
                popup.gameObject.SetActive(false);
                popupCanvasList.Add(popup);
            }

            configCanvas = UIManager.Instance.MakePopup<ConfigCanvas>(ConfigCanvas.Path);
            configCanvas.configCanvasAction -= OnPopupCanvasAction;
            configCanvas.configCanvasAction += OnPopupCanvasAction;
            configCanvas.gameObject.SetActive(false);
        }

        private void CheckLoadFiles()
        {
            if (!SaveLoadManager.Instance.IsSaveFilesExist())
            {
                menuButtons[(int)PopupType.Load].gameObject.SetActive(false);
            }
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            titleController.CheckQueue(titlePanelRect);
            menuController.CheckQueue(menuPanelRect);
#endif
        }

        #region MenuButton

        private void OnBlinkButtonAction(PopupPayload payload)
        {
            if (payload.popupType == PopupType.Config)
            {
                configCanvas.gameObject.SetActive(true);
            }
            else
            {
                int idx = (int)payload.popupType;
                popupCanvasList[idx].gameObject.SetActive(true);
            }
        }

        #endregion

        #region PopupEvent

        private void OnPopupCanvasAction(PopupPayload payload)
        {
            int idx = (int)payload.popupType;
            switch (payload.buttonType)
            {
                case ButtonType.Yes:
                    {
                        if (payload.popupType == PopupType.Start)
                        {
                            SaveLoadManager.Instance.IsLoadData = false;
                            SceneLoadManager.Instance.LoadScene(SceneName.NewStart);
                        }
                        else if (payload.popupType == PopupType.Load)
                        {
                            SaveLoadManager.Instance.IsLoadData = true;
                            SceneLoadManager.Instance.LoadScene(SceneName.InGame);
                        }
                    }
                    break;

                case ButtonType.No:
                    {
                        if (payload.popupType == PopupType.Config)
                        {
                            configCanvas.gameObject.SetActive(false);
                        }
                        else
                        {
                            popupCanvasList[idx].gameObject.SetActive(false);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}