using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class BasePopupMenu : UIPopup
    {
        public static readonly string Path = "PopupMenu";

        private enum GameObjects
        {
            PopupPanel,
            PopupTextPanel,
            PopupButtonGridPanel
        }

        [SerializeField] private UITransformData popupMenuTransform;
        [SerializeField] private UITransformData popupPanelTransform;
        [SerializeField] private UITransformData popupTextPanelTransform;
        [SerializeField] private UITransformData popupButtonGridTransform;

        private readonly TransformController popupMenuController = new TransformController();
        private readonly TransformController popupPanelController = new TransformController();
        private readonly TransformController popupTextPanelController = new TransformController();
        private readonly TransformController popupButtonGridController = new TransformController();

        private GameObject popupPanel;
        private GameObject popupTextPanel;
        private GameObject popupButtonGridPanel;

        private RectTransform menuRect;
        private RectTransform popupPanelRect;
        private RectTransform popupTextPanelRect;
        private RectTransform popupButtonGridPanelRect;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitGameObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            popupPanel = GetGameObject((int)GameObjects.PopupPanel);
            popupTextPanel = GetGameObject((int)GameObjects.PopupTextPanel);
            popupButtonGridPanel = GetGameObject((int)GameObjects.PopupButtonGridPanel);

            menuRect = gameObject.GetComponent<RectTransform>();
            popupPanelRect = popupPanel.GetComponent<RectTransform>();
            popupTextPanelRect = popupTextPanel.GetComponent<RectTransform>();
            popupButtonGridPanelRect = popupButtonGridPanel.GetComponent<RectTransform>();
        }

        private void InitGameObjects()
        {
#if UNITY_EDITOR
            popupMenuTransform.actionRect.Subscribe(popupMenuController.OnRectChange);
            popupMenuTransform.actionScale.Subscribe(popupMenuController.OnScaleChange);

            popupPanelTransform.actionRect.Subscribe(popupPanelController.OnRectChange);
            popupPanelTransform.actionScale.Subscribe(popupPanelController.OnScaleChange);

            popupTextPanelTransform.actionRect.Subscribe(popupTextPanelController.OnRectChange);
            popupTextPanelTransform.actionScale.Subscribe(popupTextPanelController.OnScaleChange);

            popupButtonGridTransform.actionRect.Subscribe(popupButtonGridController.OnRectChange);
            popupButtonGridTransform.actionScale.Subscribe(popupButtonGridController.OnScaleChange);
#endif

            AnchorPresets.SetAnchorPreset(menuRect, AnchorPresets.MiddleCenter);
            menuRect.sizeDelta = popupMenuTransform.actionRect.Value.GetSize();
            menuRect.localPosition = popupMenuTransform.actionRect.Value.ToCanvasPos();
            menuRect.localScale = popupMenuTransform.actionScale.Value;

            AnchorPresets.SetAnchorPreset(popupPanelRect, AnchorPresets.MiddleCenter);
            popupPanelRect.sizeDelta = popupPanelTransform.actionRect.Value.GetSize();
            popupPanelRect.localPosition = popupPanelTransform.actionRect.Value.ToCanvasPos();
            popupPanelRect.localScale = popupPanelTransform.actionScale.Value;

            AnchorPresets.SetAnchorPreset(popupTextPanelRect, AnchorPresets.MiddleCenter);
            popupTextPanelRect.sizeDelta = popupTextPanelTransform.actionRect.Value.GetSize();
            popupTextPanelRect.localPosition = popupTextPanelTransform.actionRect.Value.ToCanvasPos();
            popupTextPanelRect.localScale = popupTextPanelTransform.actionScale.Value;

            AnchorPresets.SetAnchorPreset(popupButtonGridPanelRect, AnchorPresets.MiddleCenter);
            popupButtonGridPanelRect.sizeDelta = popupButtonGridTransform.actionRect.Value.GetSize();
            popupButtonGridPanelRect.localPosition = popupButtonGridTransform.actionRect.Value.ToCanvasPos();
            popupButtonGridPanelRect.localScale = popupButtonGridTransform.actionScale.Value;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            popupMenuController.CheckQueue(menuRect);
            popupPanelController.CheckQueue(popupPanelRect);
            popupTextPanelController.CheckQueue(popupTextPanelRect);
            popupButtonGridController.CheckQueue(popupButtonGridPanelRect);
#endif
        }
    }
}