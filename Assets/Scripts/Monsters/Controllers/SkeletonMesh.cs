using UnityEngine;

public class SkeletonMesh : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material[] materials;
    //[SerializeField] private Material freezeMaterial;

    void Awake()
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
        Material[] oneMaterial = new Material[1];
        oneMaterial[0] = materials[0];
        skinnedMeshRenderer.materials = oneMaterial;
    }
}
