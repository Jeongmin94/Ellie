using System;
using System.Collections;
using Assets.Scripts.UI.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Player
{
    public enum FillAmountType
    {
        Background,
        Midground,
        Foreground
    }

    public class UIPlayerHealthImage : UIBase
    {
        private enum Images
        {
            Background,
            Midground,
            Foreground
        }

        [SerializeField] private Color hitColor = Color.black;
        [SerializeField] private Color emptyColor = Color.black;
        private Color originColor;

        private Image background;
        private Image midground;
        private Image foreground;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind<Image>(typeof(Images));

            background = GetImage((int)Images.Background);
            midground = GetImage((int)Images.Midground);
            foreground = GetImage((int)Images.Foreground);

            originColor = foreground.color;
            midground.color = hitColor;
            background.color = emptyColor;
        }

        public IEnumerator ChangeImageFillAmount(FillAmountType type, int total, int idx, float time,
            bool reverse = false)
        {
            Image image = GetFillAmountTarget(type);

            float timeAcc = 0.0f;
            float current = image.fillAmount;
            float target = ((reverse ? (float)(idx + 1) : (float)idx)) / (float)total;
            
            while (timeAcc <= time)
            {
                yield return null;
                timeAcc += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(current, target, timeAcc / time);
            }

            image.fillAmount = target;
        }

        public void ChangeImageFillAmount(FillAmountType type, int total, int idx, bool reverse = false)
        {
            Image image = GetFillAmountTarget(type);
            image.fillAmount = ((reverse ? (float)(idx + 1) : (float)idx)) / (float)total;
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