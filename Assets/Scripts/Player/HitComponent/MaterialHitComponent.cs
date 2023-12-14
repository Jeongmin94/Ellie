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

        private Color originColor;
        private Coroutine hitCoroutine;

        public float HitDuration() => hitDuration;

        private void Awake()
        {
            if (modelMaterial == null)
            {
                Debug.LogError($"{name}'s modelMaterial is null");
                return;
            }

            originColor = modelMaterial.GetColor(StringBaseColor);
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

                Color c = Color.Lerp(hitColor, originColor, timeAcc / returnDuration);
                modelMaterial.SetColor(StringBaseColor, c);
                modelMaterial.SetColor(StringEmissionColor, c);
            }

            modelMaterial.SetColor(StringBaseColor, originColor);
            modelMaterial.SetColor(StringEmissionColor, originColor);
        }
    }
}