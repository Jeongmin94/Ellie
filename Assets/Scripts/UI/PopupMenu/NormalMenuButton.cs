using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.PopupMenu
{
    public class NormalMenuButton : OpeningText
    {
        public new static readonly string Path = "Opening/NormalMenuText";

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnClickButton);
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        // 클릭하면 새로운 UI Popup
        protected virtual void OnClickButton(PointerEventData data)
        {
            Debug.Log($"{name} 버튼 클릭됨");
        }

        protected virtual void OnPointerEnter(PointerEventData data)
        {
        }

        protected virtual void OnPointerExit(PointerEventData data)
        {
        }
    }
}