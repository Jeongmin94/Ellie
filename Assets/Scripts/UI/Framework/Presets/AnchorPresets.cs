using UnityEngine;

namespace Assets.Scripts.UI.Framework.Presets
{
    public struct AnchorPreset
    {
        public Vector2 AnchorMin { get; set; }
        public Vector2 AnchorMax { get; set; }

        private AnchorPreset(Vector2 anchorMin, Vector2 anchorMax)
        {
            AnchorMin = anchorMin;
            AnchorMax = anchorMax;
        }

        public static AnchorPreset Of(Vector2 anchorMin, Vector2 anchorMax)
        {
            return new AnchorPreset(anchorMin, anchorMax);
        }
    }

    public static class AnchorPresets
    {
        public static void SetAnchorPreset(RectTransform transform, AnchorPreset preset)
        {
            transform.anchorMin = preset.AnchorMin;
            transform.anchorMax = preset.AnchorMax;
        }

        public static void SetAnchorPreset(Transform transform, AnchorPreset preset)
        {
            SetAnchorPreset(transform.GetComponent<RectTransform>(), preset);
        }

        public static readonly AnchorPreset StretchAll = AnchorPreset.Of(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f));

        public static readonly AnchorPreset MiddleCenter = AnchorPreset.Of(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
    }
}