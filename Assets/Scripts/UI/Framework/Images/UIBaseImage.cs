using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Framework.Images
{
    public readonly struct ImageChangeInfo
    {
        public readonly float Prev;
        public readonly float Current;
        public readonly float Target;
        public readonly float Time;
        public readonly FillAmountType Type;

        public ImageChangeInfo(float prev, float current, float target, float time,
            FillAmountType type = FillAmountType.Background)
        {
            Prev = prev;
            Current = current;
            Target = target;
            Time = time;
            Type = type;
        }
    }

    public enum FillAmountType
    {
        Background,
        Midground,
        Foreground
    }

    public class UIBaseImage : UIBase
    {
        public Color backgroundColor = Color.black;
        public Color midgroundColor = Color.black;
        public Color foregroundColor = Color.black;
        protected Image background;

        private Image changedImage;
        private float changedTarget;
        protected Image foreground;
        protected Image midground;

        public Color MidgroundColor
        {
            get { return midgroundColor; }
            set
            {
                midgroundColor = value;
                midground.color = value;
            }
        }

        public Color MidgroundStartColor { get; private set; }

        protected override void Init()
        {
            Bind<Image>(typeof(Images));

            background = GetImage((int)Images.Background);
            midground = GetImage((int)Images.Midground);
            foreground = GetImage((int)Images.Foreground);

            background.color = backgroundColor;
            MidgroundStartColor = midground.color = midgroundColor;
            foreground.color = foregroundColor;
        }

        // time 시간만큼 변경
        public virtual IEnumerator ChangeImageFillAmount(FillAmountType type, float target, float time)
        {
            var image = GetFillAmountTarget(type);

            var timeAcc = 0.0f;
            var current = image.fillAmount;

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
        public virtual void ChangeImageFillAmount(FillAmountType type, float target)
        {
            var image = GetFillAmountTarget(type);
            image.fillAmount = target;
            changedImage = image;
            changedTarget = target;
        }

        protected Image GetFillAmountTarget(FillAmountType type)
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

        private enum Images
        {
            Background,
            Midground,
            Foreground
        }
    }
}