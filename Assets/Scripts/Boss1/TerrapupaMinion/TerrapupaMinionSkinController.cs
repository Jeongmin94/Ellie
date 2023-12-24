using UnityEngine;

namespace Boss1.TerrapupaMinion
{
    public class TerrapupaMinionSkinController : MonoBehaviour
    {
        public Material material;

        private SkinnedMeshRenderer skinRenderer;

        private void Awake()
        {
            skinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        private void Start()
        {
            if (material != null)
            {
                ApplySkinMaterial();
            }
        }

        private void ApplySkinMaterial()
        {
            skinRenderer.material = material;
        }
    }
}