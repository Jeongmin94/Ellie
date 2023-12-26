using System;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class MaterialChangableObject : MonoBehaviour
    {
        private Material material;
        private Material initialMaterial;
        private Material changingMaterial;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
            initialMaterial = new Material(material);
            changingMaterial = new Material(material);
        }

        public void SetEmissionValue(float value)
        {
            Color color = GetComponent<Renderer>().material.GetColor("_EmissionColor");
            
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