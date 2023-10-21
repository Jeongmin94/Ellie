using System.Collections.Generic;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        private const string NameUIRoot = "@UI_Root";
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

        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = go.GetOrAddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true; // 각 캔버스가 서로 독립적인 sort order를 가짐

            if (sort)
                canvas.sortingOrder = order;
            else
                canvas.sortingOrder = 0;
        }

        public T MakePopup<T>(string uiName = null) where T : UIPopup
        {
            if (string.IsNullOrEmpty(uiName))
                uiName = typeof(T).Name;

            var go = ResourceManager.Instance.Instantiate($"UI/Popup/{uiName}");
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