using UnityEngine;

namespace Environments
{
    public class MaterialChangableObject : MonoBehaviour
    {
        private Material changingMaterial;
        private Material initialMaterial;
        private Material material;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
            initialMaterial = new Material(material);
            changingMaterial = new Material(material);
        }

        public void SetEmissionValue(float value)
        {
            var color = GetComponent<Renderer>().material.GetColor("_EmissionColor");

            color *= value;
            changingMaterial.SetColor("_EmissionColor", color);
            GetComponent<Renderer>().material = changingMaterial;
        }

        public void ResetMaterial()
        {
            GetComponent<Renderer>().material = initialMaterial;
        }
    }
}