using System;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigToggleText : OpeningText
    {
        public new static readonly string Path = "Opening/ConfigToggleText";
        
        [SerializeField] private Sprite hoverSprite;
        
        // !TODO: 토글 텍스트 내부에서 토글 리스트 관리

        private void Awake()
        {
            InitText();
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }
        
        private void OnPointerEnter(PointerEventData data)
        {
            SetImageSprite(hoverSprite);
        }

        private void OnPointerExit(PointerEventData data)
        {
            ResetImageSprite();
        }
    }
}