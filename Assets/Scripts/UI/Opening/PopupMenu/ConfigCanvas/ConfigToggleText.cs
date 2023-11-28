using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigToggleText : OpeningText
    {
        public new static readonly string Path = "Opening/ConfigToggleText";


        // !TODO: 토글 텍스트 내부에서 토글 리스트 관리
        public void InitConfigToggleText()
        {
            InitText();
            SetImageAlpha(0.0f);
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            SetImageAlpha(1.0f);
        }

        private void OnPointerExit(PointerEventData data)
        {
            SetImageAlpha(0.0f);
        }
    }
}