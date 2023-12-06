using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Framework.Images;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Player
{
    public class UIBarImage : UIBaseImage
    {
        private enum GameObjects
        {
            FillObjects,
            BorderObject
        }

        private const string NameMidSlider = "MidSlider";
        private const string NameForeSlider = "ForeSlider";

        private GameObject fillObjects;
        private readonly IDictionary<FillAmountType, Slider> sliders = new Dictionary<FillAmountType, Slider>();

        private Slider changedSlider;
        private float changedSliderTarget;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            fillObjects = GetGameObject((int)GameObjects.FillObjects);

            InitSliders(FillAmountType.Midground, fillObjects.transform, midground.GetComponent<RectTransform>());
            InitSliders(FillAmountType.Foreground, fillObjects.transform, foreground.GetComponent<RectTransform>());
        }

        private void InitSliders(FillAmountType type, Transform parent, RectTransform fillRect)
        {
            GameObject go = new GameObject($"{type.ToString()}Slider", typeof(Slider));
            go.transform.SetParent(parent);

            var slider = go.GetOrAddComponent<Slider>();
            slider.interactable = false;
            slider.transition = Selectable.Transition.None;
            slider.fillRect = fillRect;
            slider.maxValue = 0.0f;
            slider.maxValue = 1.0f;
            slider.value = 1.0f;

            if (!sliders.ContainsKey(type))
                sliders[type] = slider;
        }

        public override IEnumerator ChangeImageFillAmount(FillAmountType type, float target, float time)
        {
            Slider slider = null;
            if (!sliders.TryGetValue(type, out slider))
                yield break;

            float timeAcc = 0.0f;
            float current = slider.value;

            while (timeAcc <= time)
            {
                if (slider == changedSlider)
                {
                    slider.value = changedSliderTarget;
                    break;
                }

                yield return new WaitForEndOfFrame();
                timeAcc += Time.deltaTime;
                slider.value = Mathf.Lerp(current, target, timeAcc / time);
            }
        }

        public override void ChangeImageFillAmount(FillAmountType type, float target)
        {
            Slider slider = null;
            if (!sliders.TryGetValue(type, out slider))
                return;

            slider.value = target;
            changedSlider = slider;
            changedSliderTarget = target;
        }

        public IEnumerator CheckSliderValue(FillAmountType midType, FillAmountType foreType)
        {
            Slider midSlider = null;
            Slider foreSlider = null;

            if (!sliders.TryGetValue(midType, out midSlider))
                yield break;
            if (!sliders.TryGetValue(foreType, out foreSlider))
                yield break;

            while (!Mathf.Equals(midSlider.value, foreSlider.value))
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}