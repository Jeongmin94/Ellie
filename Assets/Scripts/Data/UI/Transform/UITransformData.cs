using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ActionData;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Data.UI.Transform
{
    public class TransformController
    {
        private readonly Queue<Rect> rectQueue = new Queue<Rect>();
        private readonly Queue<Vector2> scaleQueue = new Queue<Vector2>();

        public void OnRectChange(Rect rect) => rectQueue.Enqueue(rect);
        public void OnScaleChange(Vector2 scale) => scaleQueue.Enqueue(scale);

        public void CheckQueue(RectTransform owner)
        {
            if (rectQueue.Any())
            {
                var rect = rectQueue.Dequeue();
                AnchorPresets.SetAnchorPreset(owner, AnchorPresets.MiddleCenter);
                owner.sizeDelta = rect.GetSize();
                owner.localPosition = rect.ToCanvasPos();
            }

            if (scaleQueue.Any())
            {
                var scale = scaleQueue.Dequeue();
                owner.localScale = scale;
            }
        }
    }

    [CreateAssetMenu(fileName = "UITransform", menuName = "UI/TransformData", order = 0)]
    public class UITransformData : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("x, y, width, height")] public Rect rect;
        [Header("x, y scale")] public Vector2 scale = Vector2.one;

        public readonly Data<Rect> actionRect = new Data<Rect>();
        public readonly Data<Vector2> actionScale = new Data<Vector2>();

        public void OnBeforeSerialize()
        {
            rect = actionRect.Value;
            scale = actionScale.Value;
        }

        public void OnAfterDeserialize()
        {
            actionRect.Value = rect;
            actionScale.Value = scale;
        }
    }
}