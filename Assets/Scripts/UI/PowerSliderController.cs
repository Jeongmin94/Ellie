using Assets.Scripts.Data.ActionData.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PowerSliderController : MonoBehaviour
    {
        [SerializeField] private ChargingData sliderData;

        [SerializeField] private GameObject fillArea;

        [Header("Set Slider Value")]
        [SerializeField] private Color startColor = Color.white;
        [SerializeField] private Color endColor = Color.red;

        private Slider slider;
        private Image fillAreaImage;
        
        private void Awake()
        {
            slider = gameObject.GetComponent<Slider>();
            fillAreaImage = fillArea.GetComponentInChildren<Image>();
        }

        private void Start()
        {
            sliderData.ChargingValue.Subscribe(OnChangeSliderValue);
        }

        private void OnChangeSliderValue(float value)
        {
            slider.value = value / sliderData.percentages[sliderData.percentages.Length - 1];

            float r = startColor.r * (1.0f - value) + endColor.r * value;
            float g = startColor.g * (1.0f - value) + endColor.g * value;
            float b = startColor.b * (1.0f - value) + endColor.b * value;

            fillAreaImage.color = new Color(r, g, b);
        }
    }
}
