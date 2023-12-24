using UnityEngine;

namespace Monsters.Controllers
{
    public class SkeletonMesh : MonoBehaviour
    {
        private Material[] materials;

        private SkinnedMeshRenderer skinnedMeshRenderer;
        //[SerializeField] private Material freezeMaterial;

        private void Awake()
        {
            skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            materials = skinnedMeshRenderer.materials.Clone() as Material[];
            DeleteFreezeRenderer();
        }

        public void AddFreezeRenderer()
        {
            skinnedMeshRenderer.materials = materials;
        }

        public void DeleteFreezeRenderer()
        {
            var oneMaterial = new Material[1];
            oneMaterial[0] = materials[0];
            skinnedMeshRenderer.materials = oneMaterial;
        }
    }
}