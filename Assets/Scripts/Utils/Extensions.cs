using System;
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

            component = go.AddComponent<T>();
            return component;
        }
        
        public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_Base.BindEvent(go, action, type);
        }
    }
}