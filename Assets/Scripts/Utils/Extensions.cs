using System;
using Assets.Scripts.UI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Utils
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component != null)
                return component;

            return go.AddComponent<T>();
        }

        #region UI
        public static void BindEvent(this GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
        {
            // UI_Base.BindEvent(go, action, type);
        }
        #endregion
    }
}