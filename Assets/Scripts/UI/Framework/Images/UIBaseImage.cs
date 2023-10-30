using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Framework.Images
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
        private enum Images
        {
            Background,
            Midground,
            Foreground
        }

        public Color backgroundColor = Color.black;
        public Color midgroundColor = Color.black;
        public Color foregroundColor = Color.black;
        public Color MidgroundColor
        {
            get
            {
                return midgroundColor;
            }
            set
            {
                midgroundColor = value;
                midground.color = value;
            }
        }
        private Color midgroundStartColor;
        public Color MidgroundStartColor { get { return midgroundStartColor; } }
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

            background.color = backgroundColor;
            midgroundStartColor = midground.color = midgroundColor;
            foreground.color = foregroundColor;
        }

        // time 시간만큼 변경
        public virtual IEnumerator ChangeImageFillAmount(FillAmountType type, float target, float time)
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
        public virtual void ChangeImageFillAmount(FillAmountType type, float target)
        {
            Image image = GetFillAmountTarget(type);
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
    }
}