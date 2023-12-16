using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.HitComponent
{
    public class MaterialHitComponent : MonoBehaviour
    {
        private static readonly string StringBaseColor = "_BaseColor";
        private static readonly string StringEmissionColor = "_EmissionColor";

        [SerializeField] private Color hitColor = Color.red;
        [SerializeField] private float hitDuration = 0.5f;
        [SerializeField] private float returnDuration = 0.2f;
        [SerializeField] private Material modelMaterial;

        private Color baseOriginalColor;
        private Color emissionOriginalColor;
        private Coroutine hitCoroutine;

        public float HitDuration() => hitDuration;

        private void Awake()
        {
            if (modelMaterial == null)
            {
                Debug.LogError($"{name}'s modelMaterial is null");
                return;
            }

            baseOriginalColor = modelMaterial.GetColor(StringBaseColor);
            emissionOriginalColor = modelMaterial.GetColor(StringEmissionColor);
        }

        public void Hit()
        {
            if (hitCoroutine != null)
            {
                StopCoroutine(hitCoroutine);
            }

            hitCoroutine = StartCoroutine(ChangeModelMaterial());
        }

        private IEnumerator ChangeModelMaterial()
        {
            modelMaterial.SetColor(StringBaseColor, hitColor);
            modelMaterial.SetColor(StringEmissionColor, hitColor);
            yield return new WaitForSeconds(hitDuration);

            float timeAcc = 0.0f;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (timeAcc <= returnDuration)
            {
                timeAcc += Time.deltaTime;
                yield return wfef;

                Color curBaseColor = Color.Lerp(hitColor, baseOriginalColor, timeAcc / returnDuration);
                modelMaterial.SetColor(StringBaseColor, curBaseColor);
                Color curEmissionColor = Color.Lerp(hitColor, emissionOriginalColor, timeAcc / returnDuration);
                modelMaterial.SetColor(StringEmissionColor, curEmissionColor);
            }

            modelMaterial.SetColor(StringBaseColor, baseOriginalColor);
            modelMaterial.SetColor(StringEmissionColor, emissionOriginalColor);
        }
    }
}