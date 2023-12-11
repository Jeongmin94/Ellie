using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Guide
{
    public class GuideButton : UIBase
    {
        public static readonly string Path = "Guide/GuideButton";

        public void InitButton()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
            BindEvents();
        }

        private void Bind()
        {
        }

        private void InitObjects()
        {
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            gameObject.BindEvent(OnPointerExit, UIEvent.PointExit);
            gameObject.BindEvent(OnPointerClick, UIEvent.Click);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            
        }

        private void OnPointerExit(PointerEventData data)
        {
            
        }

        private void OnPointerClick(PointerEventData data)
        {
            
        }
    }
}