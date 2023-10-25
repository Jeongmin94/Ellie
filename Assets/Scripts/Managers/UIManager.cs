using System.Collections.Generic;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        private const string NameUIRoot = "@UI_Root";
        private const string PrefixPopup = "UI/Popup/";
        private const string PrefixStatic = "UI/Static/";
        private const string PrefixSubItem = "UI/SubItem/";

        private int order = 10;

        private readonly Stack<UIPopup> popupStack = new Stack<UIPopup>();

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