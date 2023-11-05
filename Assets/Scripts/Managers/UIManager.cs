using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public readonly Vector2 resolution = new Vector2(1920, 1080);

        public const string NameUIRoot = "@UI_Root";
        public const string PrefixPopup = "UI/Popup/";
        public const string PrefixStatic = "UI/Static/";
        public const string PrefixSubItem = "UI/SubItem/";

        public const string UIButtonCanvas = "ButtonCanvas";
        public const string UIHealthAndStamina = "Player/HealthAndStamina";
        public const string UIStoneInven = "Item/StoneInven";
        public const string UIItemInven = "Item/ItemInven";
        public const string UIStatusCanvas = "Player/StatusCanvas";
        public const string UIMonsterCanvas = "Monster/MonsterCanvas";
        public const string UIMonsterBillboard = "Monster/MonsterBillboard";

        // Inven
        public const string UIPopupInvenCanvas = "Inventory/PopupInvenCanvas";
        public const string UISlot = "Slot/Slot";
        public const string UISlotItem = "Slot/SlotItem";
        public const string UIGridArea = "Slot/GridArea";
        public const string UIHorizontalGridArea = "Slot/HorizontalGridArea";
        public const string UIItemMenuButton = "Slot/ItemMenuButton";

        // Inventory
        public const string Inventory = "Inventory/Inventory";
        public const string InventorySlot = "Slot/InventorySlot";
        public const string DescriptionTextPanel = "Slot/Description/DescriptionTextPanel";
        public const string DescriptionNamePanel = "Slot/Description/DescriptionNamePanel";
        public const string ImageAndTextArea = "Slot/Gold/ImageAndTextArea";

        private int order = 10;

        private readonly Stack<UIPopup> popupStack = new Stack<UIPopup>();

        public Transform OnDragParent { get; set; }

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find(NameUIRoot);
                if (root == null)
                    root = new GameObject(NameUIRoot);

                return root;
            }
        }

        public override void Awake()
        {
            base.Awake();
            if (FindObjectOfType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                eventSystem.transform.SetParent(transform);
            }
        }

        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = go.GetOrAddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true; // 각 캔버스가 서로 독립적인 sort order를 가짐
            canvas.scaleFactor = 1.0f;

            CanvasScaler canvasScaler = go.GetOrAddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = resolution;

            if (sort)
                canvas.sortingOrder = order++;
            else
                canvas.sortingOrder = 0;
        }

        public T MakeSubItem<T>(Transform parent = null, string uiName = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(uiName))
                uiName = typeof(T).Name;

            var go = ResourceManager.Instance.Instantiate($"{PrefixSubItem}{uiName}");
            if (parent)
                go.transform.SetParent(parent);

            go.transform.localScale = Vector3.one;
            return go.GetOrAddComponent<T>();
        }

        public T MakeStatic<T>(string uiName = null) where T : UIStatic
        {
            return MakeStatic<T>(Root.transform, uiName);
        }

        public T MakeStatic<T>(Transform uiParent, string uiName = null) where T : UIStatic
        {
            if (string.IsNullOrEmpty(uiName))
                uiName = typeof(T).Name;

            var go = ResourceManager.Instance.Instantiate($"{PrefixStatic}{uiName}");
            var uiStatic = go.GetOrAddComponent<T>();
            go.transform.SetParent(uiParent);

            return uiStatic;
        }

        public T MakePopup<T>(string uiName = null) where T : UIPopup
        {
            if (string.IsNullOrEmpty(uiName))
                uiName = typeof(T).Name;

            var go = ResourceManager.Instance.Instantiate($"{PrefixPopup}{uiName}");
            T popup = go.GetOrAddComponent<T>();

            popupStack.Push(popup);
            go.transform.SetParent(Root.transform);

            return popup;
        }

        public void ClosePopup(UIPopup popup)
        {
            if (popupStack.Count == 0)
                return;

            if (popupStack.Peek() != popup)
            {
                Debug.LogError($"Close Popup Failed");
                return;
            }

            ClosePopup();
        }

        public void ClosePopup()
        {
            if (popupStack.Count == 0)
                return;

            var popup = popupStack.Pop();
            ResourceManager.Instance.Destroy(popup.gameObject);
            popup = null;
            order--;
        }

        public void CloseAllPopup()
        {
            while (popupStack.Count > 0)
                ClosePopup();
        }
    }
}