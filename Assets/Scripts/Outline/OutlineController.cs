using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outline
{
    public enum OutlineType
    {
        InteractiveOutline,
        StoneOutline,
    }

    public enum OutlineAction
    {
        SetEnabled,
        SetDisabled
    }

    public class OutlineController : MonoBehaviour
    {
        private static readonly string KeyEnabled = "_Enabled";

        public List<OutlineType> outlineTypes;
        public List<Material> materials;

        public void AddOutline(MeshRenderer meshRenderer, OutlineType type)
        {
            List<Material> matList = new List<Material>(meshRenderer.materials);
            Material target = materials[(int)type];

            matList.Add(target);

            meshRenderer.materials = matList.ToArray();
        }

        public void RemoveLastMaterial(MeshRenderer meshRenderer)
        {
            List<Material> matList = new List<Material>(meshRenderer.materials);
            matList.RemoveAt(matList.Count - 1);
            meshRenderer.materials = matList.ToArray();
        }
    }
}