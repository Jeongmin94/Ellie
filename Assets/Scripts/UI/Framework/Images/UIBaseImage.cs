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

        public IEnumerator ChangeImageFillAmount(FillAmountType type, float target, float time, bool reverse = false)
        {
            Image image = GetFillAmountTarget(type);

            float timeAcc = 0.0f;
            float current = image.fillAmount;

            while (timeAcc <= time)
            {
                yield return null;
                timeAcc += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(current, target, timeAcc / time);
            }
        }
        
        public void ChangeImageFillAmount(FillAmountType type, float target, bool reverse = false)
        {
            Image image = GetFillAmountTarget(type);
            image.fillAmount = target;
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