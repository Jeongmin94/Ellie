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
    public class UIStoneInven : UIStatic
    {
        private enum Buttons
        {
            LeftButton,
            MidButton,
            RightButton,
        }

        private enum GameObjects
        {
            Left,
            Mid,
            Right
        }

        [SerializeField] private float swapTime = 0.5f;

        // ui components
        private readonly List<UIStoneSubItem> subItems = new List<UIStoneSubItem>();
        private readonly List<Button> buttons = new List<Button>();
        private readonly List<GameObject> positionObjects = new List<GameObject>();

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
            int buttonCount = Enum.GetValues(typeof(Buttons)).Length;
            for (int i = 0; i < buttonCount; i++)
            {
                buttons.Add(GetButton(i));

                var subItem = buttons[i].gameObject.GetOrAddComponent<UIStoneSubItem>();
                subItem.ItemIdx = i;
                subItem.ItemText = $"stone{i}";
                subItem.PrevPosition = buttons[i].transform.localPosition;

                subItem.PrevScale = buttons[i].transform.localScale;
                subItems.Add(subItem);

                buttons[i].gameObject.BindEvent(OnEventHandlerEvent);

                positionObjects.Add(GetGameObject(i));
            }
        }

        private void OnEventHandlerEvent(PointerEventData data)
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
            Vector3 toButton = subItem.transform.localPosition - mid.transform.localPosition;
            Vector3 toButtonPos = subItem.PrevPosition;
            Vector3 toButtonScale = subItem.PrevScale;

            // button => mid
            Vector3 toMid = mid.transform.localPosition - subItem.transform.localPosition;
            Vector3 toMidPos = mid.PrevPosition;
            Vector3 toMidScale = mid.PrevScale;

            float timeAcc = 0.0f;
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
    }
}