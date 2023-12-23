using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
    public class UIItemInven : UIStatic
    {
        [SerializeField] private Vector3 zoomScale = new(1.1f, 1.1f, 0.0f);
        [SerializeField] private Vector3 originScale = new(1.0f, 1.0f, 0.0f);

        private readonly List<UIItemSubItem> subItems = new();
        private UIItemSubItem prevSelectedItem;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));

            var itemCount = Enum.GetValues(typeof(GameObjects)).Length;
            for (var i = 0; i < itemCount; i++)
            {
                var go = GetGameObject(i);
                var button = go.FindChild<Button>();
                var subItem = button.gameObject.GetOrAddComponent<UIItemSubItem>();

                subItem.gameObject.BindEvent(OnClickItem);
                subItem.transform.localScale = originScale;

                subItems.Add(subItem);
            }
        }

        private void OnClickItem(PointerEventData data)
        {
            if (prevSelectedItem)
            {
                prevSelectedItem.transform.localScale = originScale;
            }

            // !TODO: 선택한 아이템을 사용 혹은 장착하도록 하는 단계 추가 필요
            // 아이템 선택
            var selected = data.pointerEnter.gameObject;
            var subItem = selected.GetComponent<UIItemSubItem>();
            subItem.transform.localScale = zoomScale;

            prevSelectedItem = subItem;
        }

        // left: w, h 늘어난만큼 posX 감소
        // top: w, h 늘어난만큼 posY 증가
        // right: w, h 늘어난만큼 posX 증가
        // bottom: w, h 늘어난만큼 posY 감소
        private enum GameObjects
        {
            Left,
            Top,
            Right,
            Bottom
        }

        // private IEnumerator ScaleButton(UIItemSubItem item)
        // {
        //     
        // }
    }
}