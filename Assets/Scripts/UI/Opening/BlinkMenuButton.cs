using System.Collections;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Opening
{
    public class BlinkMenuButton : OpeningText
    {
        public new static readonly string Path = "Opening/MenuText";

        [SerializeField] private float blinkInterval = 1.0f;

        private ColorBlock colorBlock;
        private bool isBlink = false;

        protected override void Init()
        {
            base.Init();

            Color color = OriginColor();
            color.a = 0.0f;
            SetOriginColor(color);
            imageColor.Value = color;
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnClickButton);
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        public override void InitTypography(TextTypographyData typographyTypographyData)
        {
            base.InitTypography(typographyTypographyData);

            colorBlock.highlightedColor = typographyTypographyData.highlightedColor;
            colorBlock.pressedColor = typographyTypographyData.pressedColor;
            colorBlock.selectedColor = typographyTypographyData.selectedColor;
            colorBlock.disabledColor = typographyTypographyData.disabledColor;
        }

        // 클릭하면 새로운 UI Popup
        protected virtual void OnClickButton(PointerEventData data)
        {
            Debug.Log($"{name} 버튼 클릭됨");
        }

        protected virtual void OnPointerEnter(PointerEventData data)
        {
            StartCoroutine(BlinkPanelImage());
        }

        protected virtual void OnPointerExit(PointerEventData data)
        {
            isBlink = false;
        }

        private IEnumerator BlinkPanelImage()
        {
            isBlink = true;
            Color color = OriginColor();
            color.a = 0.0f;
            imageColor.Value = color;

            bool toRight = true;
            float timeAcc = 0.0f;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (isBlink)
            {
                yield return wfef;

                float alpha = Mathf.Lerp(0.0f, 1.0f, timeAcc / blinkInterval);
                color.a = alpha;
                imageColor.Value = color;

                if (toRight)
                {
                    timeAcc += Time.deltaTime;
                }
                else
                {
                    timeAcc -= Time.deltaTime;
                }

                if (timeAcc > blinkInterval)
                    toRight = false;
                else if (timeAcc <= 0.0f)
                    toRight = true;
            }

            ResetImageColor();
        }
    }
}