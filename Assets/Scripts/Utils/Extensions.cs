using Assets.Scripts.UI.Framework;
using System;
using System.Linq;
using Assets.Scripts.UI.PopupMenu;
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
                    handler.clickHandlerAction -= action;
                    handler.clickHandlerAction += action;
                }
                    break;
                case UIEvent.Drag:
                {
                    handler.dragHandlerAction -= action;
                    handler.dragHandlerAction += action;
                }
                    break;
                case UIEvent.BeginDrag:
                {
                    handler.beginDragHandlerAction -= action;
                    handler.beginDragHandlerAction += action;
                }
                    break;
                case UIEvent.EndDrag:
                {
                    handler.endDragHandlerAction -= action;
                    handler.endDragHandlerAction += action;
                }
                    break;

                case UIEvent.Drop:
                {
                    handler.dropHandlerAction -= action;
                    handler.dropHandlerAction += action;
                }
                    break;

                case UIEvent.Down:
                {
                    handler.downHandlerAction -= action;
                    handler.downHandlerAction += action;
                }
                    break;

                case UIEvent.Up:
                {
                    handler.upHandlerAction -= action;
                    handler.upHandlerAction += action;
                }
                    break;

                case UIEvent.PointEnter:
                {
                    handler.pointerEnterAction -= action;
                    handler.pointerEnterAction += action;
                }
                    break;

                case UIEvent.PointExit:
                {
                    handler.pointerExitAction -= action;
                    handler.pointerExitAction += action;
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

        public static PopupCanvas AddPopupCanvas(this GameObject go, PopupType popupType)
        {
            PopupCanvas popupCanvas = null;
            switch (popupType)
            {
                case PopupType.Load:
                    popupCanvas = go.GetOrAddComponent<LoadPopup>();
                    break;
                case PopupType.Start:
                    popupCanvas = go.GetOrAddComponent<StartPopup>();
                    break;
                case PopupType.Config:
                    popupCanvas = go.GetOrAddComponent<ConfigPopup>();
                    break;
                case PopupType.Exit:
                    popupCanvas = go.GetOrAddComponent<ExitPopup>();
                    break;
                case PopupType.Main:
                    popupCanvas = go.GetOrAddComponent<MainPopup>();
                    break;
                case PopupType.Escape:
                    popupCanvas = go.GetOrAddComponent<EscapePopup>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(popupType), popupType, null);
            }

            return popupCanvas;
        }

        #endregion
    }
}