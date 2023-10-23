using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Framework.Images
{
    public enum FillAmountType
    {
        Background,
        Midground,
        Foreground
    }

    public class UIBaseImage : UIBase
    {
        private enum Images
        {
            Background,
            Midground,
            Foreground
        }

        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeField] private Color midgroundColor = Color.black;
        [SerializeField] private Color foregroundColor = Color.black;

        protected Image background;
        protected Image midground;
        protected Image foreground;

        private Image changedImage;
        private float changedTarget;

        protected override void Init()
        {
            Bind<Image>(typeof(Images));

            background = GetImage((int)Images.Background);
            midground = GetImage((int)Images.Midground);
            foreground = GetImage((int)Images.Foreground);

            backgroundColor = background.color;
            midgroundColor = midground.color;
            foregroundColor = foreground.color;
        }

        // time 시간만큼 변경
        public IEnumerator ChangeImageFillAmount(FillAmountType type, float target, float time)
        {
            Image image = GetFillAmountTarget(type);

            float timeAcc = 0.0f;
            float current = image.fillAmount;

            while (timeAcc <= time)
            {
                if (image == changedImage)
                {
                    image.fillAmount = changedTarget;
                    break;
                }

                yield return new WaitForEndOfFrame();
                timeAcc += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(current, target, timeAcc / time);
            }
        }

        // 즉시 변경
        public void ChangeImageFillAmount(FillAmountType type, float target)
        {
            Image image = GetFillAmountTarget(type);
            image.fillAmount = target;
            changedImage = image;
            changedTarget = target;
        }

        private Image GetFillAmountTarget(FillAmountType type)
        {
            Image image = null;
            switch (type)
            {
                case FillAmountType.Background:
                    image = background;
                    break;
                case FillAmountType.Midground:
                    image = midground;
                    break;
                case FillAmountType.Foreground:
                    image = foreground;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return image;
        }
    }
}