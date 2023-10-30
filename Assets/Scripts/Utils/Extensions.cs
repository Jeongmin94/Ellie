using System;
using System.Linq;
using Assets.Scripts.UI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Utils
{
    public static class Extensions
    {
        #region GameObject

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            return component != null ? component : go.AddComponent<T>();
        }

        public static GameObject FindChild(this GameObject go, string name = null, bool recursive = false)
        {
            var transform = FindChild<Transform>(go, name, recursive);
            return transform ? transform.gameObject : null;
        }

        public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : Object
        {
            if (go == null)
                return null;

            if (recursive)
            {
                return go.GetComponentsInChildren<T>()
                    .FirstOrDefault(component => string.IsNullOrEmpty(name) || component.name.Equals(name));
            }

            for (int i = 0; i < go.transform.childCount; i++)
            {
                var transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name.Equals(name))
                {
                    var component = transform.GetComponent<T>();
                    if (component)
                        return component;
                }
            }

            return null;
        }
        
        #endregion

        #region UI

        /// <summary>
        /// UI GameObject에 UIEvent를 바인딩
        /// </summary>
        /// <param name="go"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        public static void BindEvent(this GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
        {
            var handler = go.GetOrAddComponent<UIEventHandler>();

            switch (type)
            {
                case UIEvent.Click:
                {
                    handler.OnClickHandler -= action;
                    handler.OnClickHandler += action;
                }
                    break;
                case UIEvent.Drag:
                {
                    handler.OnDragHandler -= action;
                    handler.OnDragHandler += action;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static void SetAllPadding(this RectOffset offset, int value)
        {
            offset.bottom = value;
            offset.top = value;
            offset.left = value;
            offset.right = value;
        }

        #endregion
    }
}