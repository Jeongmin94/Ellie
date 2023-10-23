using System;
using System.Collections;
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

        [SerializeField] private Vector3 zoomScale = new Vector3(1.1f, 1.1f, 0.0f);
        [SerializeField] private Vector3 originScale = new Vector3(1.0f, 1.0f, 0.0f);

        private readonly List<UIItemSubItem> subItems = new List<UIItemSubItem>();
        private UIItemSubItem prevSelectedItem;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));

            int itemCount = Enum.GetValues(typeof(GameObjects)).Length;
            for (int i = 0; i < itemCount; i++)
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
                prevSelectedItem.transform.localScale = originScale;

            // 아이템 선택
            var selected = data.pointerEnter.gameObject;
            Debug.Log($"selected item: {selected.name}");
            var subItem = selected.GetComponent<UIItemSubItem>();
            subItem.transform.localScale = zoomScale;

            prevSelectedItem = subItem;
        }

        // private IEnumerator ScaleButton(UIItemSubItem item)
        // {
        //     
        // }
    }
}