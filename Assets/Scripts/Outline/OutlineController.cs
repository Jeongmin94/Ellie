using System.Collections.Generic;
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

        private readonly List<Material> buffer = new List<Material>();

        public void AddOutline(Renderer targetRenderer, OutlineType type)
        {
            buffer.Clear();

            buffer.AddRange(targetRenderer.sharedMaterials);
            if (buffer.Contains(materials[(int)type]))
                return;

            buffer.Add(materials[(int)type]);

            targetRenderer.materials = buffer.ToArray();
        }

        public void RemoveMaterial(Renderer targetRenderer, OutlineType type)
        {
            buffer.Clear();

            buffer.AddRange(targetRenderer.sharedMaterials);
            if (!buffer.Contains(materials[(int)type]))
                return;

            buffer.Remove(materials[(int)type]);

            targetRenderer.materials = buffer.ToArray();
        }
    }
}