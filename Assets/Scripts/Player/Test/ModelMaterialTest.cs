using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Test
{
    public class ModelMaterialTest : MonoBehaviour
    {
        private static readonly string BaseColor = "_BaseColor";

        public Color hitColor;
        public float hitDuration = 0.5f;
        public float returnDuration = 0.2f;

        private Material modelMaterial;

        private Color originColor;

        private void Start()
        {
            Debug.Log($"{name} - Start");
            modelMaterial = GetComponent<SkinnedMeshRenderer>().material;

            Debug.Log($"material: {modelMaterial.name}");

            originColor = modelMaterial.GetColor(BaseColor);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(Hit());
            }
        }

        private IEnumerator Hit()
        {
            Debug.Log($"{name} Hit");
            modelMaterial.SetColor(BaseColor, hitColor);
            yield return new WaitForSeconds(hitDuration);

            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();
            while (timeAcc <= returnDuration)
            {
                timeAcc += Time.deltaTime;
                yield return wfef;

                var c = Color.Lerp(hitColor, originColor, timeAcc / returnDuration);
                modelMaterial.SetColor(BaseColor, c);
            }

            modelMaterial.SetColor(BaseColor, originColor);
        }
    }
}