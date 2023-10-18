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

        private readonly float maxChargingTime = 1.5f;
        private float chargingTime = 0.0f;
        private bool onCharge = false;

        private void Awake()
        {
            slider = gameObject.GetComponent<Slider>();
            fillAreaImage = fillArea.GetComponentInChildren<Image>();
        }

        private void Start()
        {
            sliderData.ChargingValue.OnChange -= OnChangeSliderValue;
            sliderData.ChargingValue.OnChange += OnChangeSliderValue;
        }

        // !TODO: 차징하는 주체는 Shooter로 변경
        private void Update()
        {
            //Charge();
        }

        private void Charge()
        {
            if (Input.GetMouseButtonDown(0) && !onCharge)
            {
                onCharge = true;
                chargingTime += Time.deltaTime / Time.timeScale;
                sliderData.ChargingValue.Value = chargingTime / maxChargingTime;
            }
            else if (Input.GetMouseButton(0) && onCharge)
            {
                chargingTime = Mathf.Clamp(chargingTime + Time.deltaTime / Time.timeScale, 0.0f, maxChargingTime);
                sliderData.ChargingValue.Value = chargingTime / maxChargingTime;
            }
            else if (Input.GetMouseButtonUp(0) && onCharge)
            {
                onCharge = false;
                sliderData.ChargingValue.Value = chargingTime / maxChargingTime;

                chargingTime = 0.0f;
                slider.value = 0.0f;
                fillAreaImage.color = startColor;
            }
        }

        private void OnChangeSliderValue(float value)
        {
            slider.value = value;

            float r = startColor.r * (1.0f - value) + endColor.r * value;
            float g = startColor.g * (1.0f - value) + endColor.g * value;
            float b = startColor.b * (1.0f - value) + endColor.b * value;

            fillAreaImage.color = new Color(r, g, b);
        }
    }
}
