using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.Utils;
using TMPro;
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

        private enum Texts
        {
            LeftText,
            MidText,
            RightText
        }

        private const string NameLeftText = "LeftButton";
        private const string NameMidText = "MidText";
        private const string NameRightText = "RightText";

        private List<int> stones = new List<int>();
        private int current = 1;

        private Button leftButton;
        private Button midButton;
        private Button rightButton;

        private TextMeshProUGUI leftText;
        private TextMeshProUGUI midText;
        private TextMeshProUGUI rightText;

        private Vector3 leftOriginPosition;
        private Vector3 midOriginPosition;
        private Vector3 rightOriginPosition;

        private Vector3 leftOriginScale;
        private Vector3 midOriginScale;
        private Vector3 rightOriginScale;

        private void Awake()
        {
            stones.Add(1);
            stones.Add(2);
            stones.Add(3);
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<TextMeshProUGUI>(typeof(Texts));

            leftButton = GetButton((int)Buttons.LeftButton);
            midButton = GetButton((int)Buttons.MidButton);
            rightButton = GetButton((int)Buttons.RightButton);

            leftOriginPosition = leftButton.transform.position;
            midOriginPosition = midButton.transform.position;
            rightOriginPosition = rightButton.transform.position;

            leftOriginScale = leftButton.transform.localScale;
            midOriginScale = midButton.transform.localScale;
            rightOriginScale = rightButton.transform.localScale;

            leftButton.gameObject.BindEvent(OnEventHandlerEvent);
            midButton.gameObject.BindEvent(OnEventHandlerEvent);
            rightButton.gameObject.BindEvent(OnEventHandlerEvent);

            leftText = GetText((int)Texts.LeftText);
            midText = GetText((int)Texts.MidText);
            rightText = GetText((int)Texts.RightText);

            leftText.text = stones[0].ToString();
            midText.text = stones[1].ToString();
            rightText.text = stones[2].ToString();
        }

        private void OnEventHandlerEvent(PointerEventData data)
        {
            Debug.Log($"{data.pointerEnter.gameObject.name}");
            string selected = data.pointerEnter.gameObject.name;

            if (selected.Equals(NameLeftText))
            {
                StartCoroutine(SwapStone());
            }
            else if (selected.Equals(NameRightText))
            {
                current = 2;
            }
            else
            {
            }
        }

        private float time = 0.2f;

        private IEnumerator SwapStone()
        {
            // mid => left
            Vector3 midToLeft = leftButton.transform.position - midButton.transform.position;
            float d1 = midToLeft.magnitude;
            Vector3 midTargetPos = leftOriginPosition;
            Vector3 midTargetScale = leftOriginScale;

            // left => mid
            Vector3 leftToMid = midButton.transform.position - leftButton.transform.position;
            float d2 = leftToMid.magnitude;
            Vector3 leftTargetPos = midOriginPosition;
            Vector3 leftTargetScale = midOriginScale;

            float timeAcc = 0.0f;
            while (timeAcc <= time)
            {
                yield return null;
                timeAcc += Time.deltaTime;

                midButton.transform.position += midToLeft.normalized * ((d1 / time) * Time.deltaTime);
                midButton.transform.localScale = Vector3.Lerp(midOriginScale, leftOriginScale, timeAcc / time);

                leftButton.transform.position += leftToMid.normalized * ((d2 / time) * Time.deltaTime);
                leftButton.transform.localScale = Vector3.Lerp(leftOriginScale, midOriginScale, timeAcc / time);
            }

            midButton.transform.position = midTargetPos;
            midButton.transform.localScale = midTargetScale;

            leftButton.transform.position = leftTargetPos;
            leftButton.transform.localScale = leftTargetScale;
        }
    }
}