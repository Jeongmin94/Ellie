using System;
using System.Collections;
using System.Collections.Generic;
using UI.Framework.Static;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Item.Stone
{
    public class UIStoneInven : UIStatic
    {
        [SerializeField] private float swapTime = 0.5f;
        private readonly List<Button> buttons = new();

        // ui components
        private readonly List<UIStoneSubItem> subItems = new();

        // for swap
        private int equippedNumber = 1;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<GameObject>(typeof(GameObjects));

            // !TODO: 플레이어가 돌멩이를 획득한 상황에 따라서 subItem의 상황이 달라짐
            var buttonCount = Enum.GetValues(typeof(Buttons)).Length;
            for (var i = 0; i < buttonCount; i++)
            {
                buttons.Add(GetButton(i));

                var subItem = buttons[i].gameObject.GetOrAddComponent<UIStoneSubItem>();
                subItem.ItemIdx = i;
                subItem.ItemText = $"stone{i}";
                subItem.PrevPosition = buttons[i].transform.localPosition;

                subItem.PrevScale = buttons[i].transform.localScale;
                subItems.Add(subItem);

                buttons[i].gameObject.BindEvent(OnClickStone);
            }
        }

        private void OnClickStone(PointerEventData data)
        {
            var selected = data.pointerEnter.gameObject;
            if (selected.name.Equals(buttons[equippedNumber].name))
            {
                return;
            }

            var subItem = selected.GetComponent<UIStoneSubItem>();
            if (subItem.ItemImage.enabled)
            {
                StartCoroutine(SwapStone(subItem));
                equippedNumber = subItem.ItemIdx;
            }
        }

        // equipped button <-> idx button swap
        private IEnumerator SwapStone(UIStoneSubItem subItem)
        {
            subItems.ForEach(i => i.ItemImage.enabled = false);
            var mid = subItems[equippedNumber];

            // mid => button
            var toButton = subItem.transform.localPosition - mid.transform.localPosition;
            var toButtonPos = subItem.PrevPosition;
            var toButtonScale = subItem.PrevScale;

            // button => mid
            var toMid = mid.transform.localPosition - subItem.transform.localPosition;
            var toMidPos = mid.PrevPosition;
            var toMidScale = mid.PrevScale;

            var timeAcc = 0.0f;
            while (timeAcc <= swapTime)
            {
                yield return new WaitForEndOfFrame();
                timeAcc += Time.deltaTime;

                mid.transform.localPosition += toButton.normalized * (toButton.magnitude / swapTime * Time.deltaTime);
                mid.transform.localScale = Vector3.Lerp(toMidScale, toButtonScale, timeAcc / swapTime);

                subItem.transform.localPosition += toMid.normalized * (toMid.magnitude / swapTime * Time.deltaTime);
                subItem.transform.localScale = Vector3.Lerp(toButtonScale, toMidScale, timeAcc / swapTime);
            }

            mid.PrevPosition = toButtonPos;
            mid.PrevScale = toButtonScale;
            mid.transform.localPosition = toButtonPos;
            mid.transform.localScale = toButtonScale;

            subItem.PrevPosition = toMidPos;
            subItem.PrevScale = toMidScale;
            subItem.transform.localPosition = toMidPos;
            subItem.transform.localScale = toMidScale;

            subItems.ForEach(i => i.ItemImage.enabled = true);
        }

        private enum Buttons
        {
            LeftButton,
            MidButton,
            RightButton
        }

        private enum GameObjects
        {
            Left,
            Mid,
            Right
        }
    }
}