using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.HitComponent
{
    public class MaterialHitComponent : MonoBehaviour
    {
        private static readonly string StringBaseColor = "_BaseColor";
        private static readonly string StringEmissionColor = "_EmissionColor";

        [SerializeField][Required] private Material modelMaterial;
        [SerializeField] private Color hitColor = Color.red;
        [SerializeField] private float hitDuration = 0.5f;
        [SerializeField] private float returnDuration = 0.2f;

        public Color BaseOriginalColor { get; private set; }
        public Color EmissionOriginalColor { get; private set; }
        public Coroutine BaseCoroutine { get; private set; }
        public Coroutine EmissionCoroutine { get; private set; }

        public float HitDuration() => hitDuration;

        private void Awake()
        {
            if (modelMaterial == null)
            {
                Debug.LogError($"{name}'s modelMaterial is null");
                return;
            }

            BaseOriginalColor = modelMaterial.GetColor(StringBaseColor);
            EmissionOriginalColor = modelMaterial.GetColor(StringEmissionColor);
        }

        private void OnDisable()
        {
            StopAllCoroutines();

            SetOriginalColor();
        }

        public void Hit()
        {
            if (BaseCoroutine != null || EmissionCoroutine != null)
            {
                SetOriginalColor();
            }

            SetBaseColor(BaseOriginalColor, hitColor, hitDuration, returnDuration);
            SetEmissionColor(EmissionOriginalColor, hitColor, hitDuration, returnDuration);
        }

        public void SetBaseColor(Color targetColor, Color startColor, float hitDuration, float returnDuration)
        {
            if (BaseCoroutine != null)
            {
                StopCoroutine(BaseCoroutine);
            }

            BaseCoroutine = StartCoroutine(ChangeColorCoroutine(StringBaseColor, targetColor, startColor, hitDuration, returnDuration));
        }

        public void SetEmissionColor(Color targetColor, Color startColor, float hitDuration, float returnDuration)
        {
            if (EmissionCoroutine != null)
            {
                StopCoroutine(EmissionCoroutine);
            }

            EmissionCoroutine = StartCoroutine(ChangeColorCoroutine(StringEmissionColor, targetColor, startColor, hitDuration, returnDuration));
        }

        private IEnumerator ChangeColorCoroutine(string colorPropertyName, Color targetColor, Color startColor, float hitDuration, float returnDuration)
        {
            modelMaterial.SetColor(colorPropertyName, startColor);
            yield return new WaitForSeconds(hitDuration);

            float timeAcc = 0.0f;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (timeAcc <= returnDuration)
            {
                timeAcc += Time.deltaTime;
                yield return wfef;

                Color curColor = Color.Lerp(startColor, targetColor, timeAcc / returnDuration);
                modelMaterial.SetColor(colorPropertyName, curColor);
            }

            modelMaterial.SetColor(colorPropertyName, targetColor);
        }

        public void SetOriginalColor()
        {
            modelMaterial.SetColor(StringBaseColor, BaseOriginalColor);
            modelMaterial.SetColor(StringEmissionColor, EmissionOriginalColor);
        }
    }
}