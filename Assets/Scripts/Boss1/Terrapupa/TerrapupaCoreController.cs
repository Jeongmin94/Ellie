using Assets.Scripts.Player.HitComponent;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Scripts.Boss1.Terrapupa
{
    public class TerrapupaCoreController : SerializedMonoBehaviour
    {
        public float blinkIntensity = 4.0f;
        public float blinkDuration = 0.45f;
        public float darkenDuration = 2.0f;

        private MaterialHitComponent hitComponent;
        private VisualEffect coreEffect;
        private Coroutine blinkCoroutine;
        private bool isBlinking;

        private void Awake()
        {
            coreEffect = GetComponentInChildren<VisualEffect>();
            hitComponent = GetComponent<MaterialHitComponent>();
        }

        private void Start()
        {
            coreEffect.enabled = false;
        }

        public void PlayCoreEffect()
        {
            coreEffect.enabled = true;
            coreEffect.Play();
        }

        public void StopCoreEffect()
        {
            coreEffect.Stop();
        }

        [Button]
        public void StartBlinkCore()
        {
            isBlinking = true;
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkCoroutine());
            }
        }

        [Button]
        public void StopBlinkCore()
        {
            isBlinking = false;
            blinkCoroutine = null;
        }

        [Button]
        public void DarkenCore()
        {
            StopAllCoroutines();
            StopCoreEffect();
            StartCoroutine(DarkenCoroutine());
        }

        private IEnumerator BlinkCoroutine()
        {
            while (isBlinking)
            {
                var originColor = hitComponent.EmissionOriginalColor;

                hitComponent.SetEmissionColor(originColor * blinkIntensity, originColor, 0.0f, blinkDuration);
                yield return hitComponent.EmissionCoroutine;

                hitComponent.SetEmissionColor(originColor, originColor * blinkIntensity, 0.0f, blinkDuration);
                yield return hitComponent.EmissionCoroutine; 
            }
        }

        private IEnumerator DarkenCoroutine()
        {
            var originColor = hitComponent.EmissionOriginalColor;

            hitComponent.SetEmissionColor(originColor * 0.0f, originColor, 0.0f, darkenDuration);
            yield return hitComponent.EmissionCoroutine;
        }
    }
}