using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class FreezeRenderSetting : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        private Material[] materials;

        void Awake()
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            materials = meshRenderer.materials.Clone() as Material[];
            DeleteFreezeRenderer();
        }

        public void AddFreezeRenderer()
        {
            meshRenderer.materials = materials;
        }
        public void DeleteFreezeRenderer()
        {
            Material[] oneMaterial = new Material[1];
            oneMaterial[0] = materials[0];
            meshRenderer.materials = oneMaterial;
        }
    }
}